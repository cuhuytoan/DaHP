using Blazored.Toast;
using CMS.Website.Areas.Identity;
using CMS.Website.NotiHub;
using CMS.Website.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(x => x.MaximumReceiveMessageSize = 102400000);
// ===== Add Database ===============================
builder.Services.ConfigureConnectDB(builder.Configuration.GetConnectionString("CmsConnection"));
// ===== Add Database Auth===========================
builder.Services.ConfigureConnectDBAuth(builder.Configuration.GetConnectionString("AuthConnection"));
// ===== Config Identity=============================
builder.Services.ConfigureDefaultIdentity();
//services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
// ===== Config Token Life Span=============================
builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<IdentityUser>>();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
//services.AddDatabaseDeveloperPageExceptionFilter();
// ===== Add Logging ================================
builder.Services.ConfigureLogging();

// ===== Config Payment ================================


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie()
      .AddGoogle(o =>
      {
          o.ClientId = builder.Configuration["Authentication:Google:ClientId"];
          o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
      })
    .AddFacebook(fo =>
    {
        fo.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        fo.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });

// ===== Add RazorPage Authorize=====================
builder.Services.ConfigureRazorPagesAuthorize();
// ===== Config AutoMaper ===========================
builder.Services.AddAutoMapper(typeof(Program));
// Add the Kendo UI services to the services container.
builder.Services.AddTelerikBlazor();
// ===== Add DI Services Wraper =====================
//services.ConfigureRepositoryWrapper();
// ===== Add Services Transient Repository===========
builder.Services.ConfigureTransient();
// ===== Add Toast Blazor===========
builder.Services.AddBlazoredToast();
// ===== Add Noti===========
builder.Services.AddSignalR();
// ===== Add Session===========
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10 * 60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//for signalR
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
// ===== Antiforgery razor ===========            
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
// ===== Local Storage  ===========  
//services.AddBlazoredLocalStorage();   // local storage
//services.AddBlazoredLocalStorage(config =>
//    config.JsonSerializerOptions.WriteIndented = true);  // local storage

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMemoryCache();





var app = builder.Build();
//for signalR
app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//Image Resize Middle Ware
app.UseMiddleware<ImageResizerMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<BlazorCookieLoginMiddleware>();

//LogVisit Middle Ware
app.UseMiddleware<LogVisitMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapHub<NotificationHubs>("/notificationHubs");
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapFallbackToAreaPage("/Admin/{*clientroutes:nonfile}", "/_HostAdmin", "Admin");
    endpoints.MapFallbackToAreaPage("/Shopman/{*clientroutes:nonfile}", "/_HostShopman", "Shopman");
    endpoints.MapFallbackToAreaPage("/Member/{*clientroutes:nonfile}", "/_HostMember", "Member");
});

app.Run();
