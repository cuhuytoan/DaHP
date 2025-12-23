using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "advertisingblock",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertisingblock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "advertisingtype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertisingtype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "advise",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advise", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "articleblock",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articlecategoryid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    style_id = table.Column<long>(type: "bigint", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articleblock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "articlecategory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    displaymenu = table.Column<bool>(type: "boolean", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlecategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlecategory_articlecategory_parentid",
                        column: x => x.parentid,
                        principalTable: "articlecategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "articlestatus",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlestatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "articletype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articletype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aspnetroles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    aspnetrolegroup = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrencystamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetroles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bank",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    shortname = table.Column<string>(type: "text", nullable: true),
                    logo = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    phonecode = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departmentman",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departmentman", x => x.id);
                    table.ForeignKey(
                        name: "FK_departmentman_departmentman_parentid",
                        column: x => x.parentid,
                        principalTable: "departmentman",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "logvisit",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    objecttype = table.Column<string>(type: "text", nullable: true),
                    objecttypename = table.Column<string>(type: "text", nullable: true),
                    objectid = table.Column<string>(type: "text", nullable: true),
                    objectname = table.Column<string>(type: "text", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandname = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    sessionid = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    agentid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logvisit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "newsletter_subscription",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    unsubscribedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newsletter_subscription", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productblock",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productcategoryid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    style_id = table.Column<long>(type: "bigint", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productblock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productbrandcategory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    departmentmanid = table.Column<long>(type: "bigint", nullable: true),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    displaymenu = table.Column<bool>(type: "boolean", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrandcategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_productbrandcategory_productbrandcategory_parentid",
                        column: x => x.parentid,
                        principalTable: "productbrandcategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productbrandlevel",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrandlevel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productbrandstatus",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrandstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productbrandtype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrandtype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productcategory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    displaymenu = table.Column<bool>(type: "boolean", nullable: true),
                    displaymenuhorizontal = table.Column<bool>(type: "boolean", nullable: true),
                    menucolor = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    metadescription = table.Column<string>(type: "text", nullable: true),
                    metakeywords = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_productcategory_productcategory_parentid",
                        column: x => x.parentid,
                        principalTable: "productcategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productmanufacture",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productmanufacture", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productorderpaymentmethod",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productorderpaymentmethod", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productorderpaymentstatus",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    color = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productorderpaymentstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productorderstatus",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    color = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productorderstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productpropertycategory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productcategoryid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productpropertycategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productpropertytype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    templatedisplay = table.Column<string>(type: "text", nullable: true),
                    templateedit = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productpropertytype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productstatus",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productstatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "producttype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producttype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "replacechar",
                columns: table => new
                {
                    replacechar_id = table.Column<long>(type: "bigint", nullable: false),
                    oldchar = table.Column<string>(type: "text", nullable: true),
                    newchar = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_replacechar", x => x.replacechar_id);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    domain = table.Column<string>(type: "text", nullable: true),
                    websitename = table.Column<string>(type: "text", nullable: true),
                    adminname = table.Column<string>(type: "text", nullable: true),
                    emailsupport = table.Column<string>(type: "text", nullable: true),
                    emailorder = table.Column<string>(type: "text", nullable: true),
                    emailsendersmtp = table.Column<string>(type: "text", nullable: true),
                    emailsenderport = table.Column<string>(type: "text", nullable: true),
                    emailsenderssl = table.Column<bool>(type: "boolean", nullable: true),
                    emailsender = table.Column<string>(type: "text", nullable: true),
                    emailsenderpassword = table.Column<string>(type: "text", nullable: true),
                    telephone = table.Column<string>(type: "text", nullable: true),
                    appstatus = table.Column<bool>(type: "boolean", nullable: false),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    defaultlanguage_id = table.Column<long>(type: "bigint", nullable: false),
                    defaultskin_id = table.Column<long>(type: "bigint", nullable: false),
                    metadescriptiondefault = table.Column<string>(type: "text", nullable: true),
                    metakeywordsdefault = table.Column<string>(type: "text", nullable: true),
                    metatitledefault = table.Column<string>(type: "text", nullable: true),
                    googleanalyticscode = table.Column<string>(type: "text", nullable: true),
                    othercode = table.Column<string>(type: "text", nullable: true),
                    facebookpageid = table.Column<string>(type: "text", nullable: true),
                    facebookappid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sysdiagrams",
                columns: table => new
                {
                    diagram_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    principal_id = table.Column<long>(type: "bigint", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: true),
                    definition = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sysdiagrams", x => x.diagram_id);
                });

            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "advertising",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    advertisingblockid = table.Column<long>(type: "bigint", nullable: true),
                    advertisingtypeid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditedby = table.Column<string>(type: "text", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertising", x => x.id);
                    table.ForeignKey(
                        name: "FK_advertising_advertisingblock_advertisingblockid",
                        column: x => x.advertisingblockid,
                        principalTable: "advertisingblock",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_advertising_advertisingtype_advertisingtypeid",
                        column: x => x.advertisingtypeid,
                        principalTable: "advertisingtype",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "articlecategoryassign",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articlecategoryid = table.Column<long>(type: "bigint", nullable: true),
                    aspnetusersid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlecategoryassign", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlecategoryassign_articlecategory_articlecategoryid",
                        column: x => x.articlecategoryid,
                        principalTable: "articlecategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "aspnetroleclaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<string>(type: "text", nullable: false),
                    claimtype = table.Column<string>(type: "text", nullable: true),
                    claimvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetroleclaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_aspnetroleclaims_aspnetroles_roleid",
                        column: x => x.roleid,
                        principalTable: "aspnetroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    countryid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_location_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productcategoryassign",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productcategoryid = table.Column<long>(type: "bigint", nullable: true),
                    aspnetusersid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcategoryassign", x => x.id);
                    table.ForeignKey(
                        name: "FK_productcategoryassign_productcategory_productcategoryid",
                        column: x => x.productcategoryid,
                        principalTable: "productcategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productproperty",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productcategoryid = table.Column<long>(type: "bigint", nullable: true),
                    productpropertycategoryid = table.Column<long>(type: "bigint", nullable: true),
                    productpropertytypeid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    unitid = table.Column<long>(type: "bigint", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productproperty", x => x.id);
                    table.ForeignKey(
                        name: "FK_productproperty_productpropertycategory_productpropertycate~",
                        column: x => x.productpropertycategoryid,
                        principalTable: "productpropertycategory",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productproperty_productpropertytype_productpropertytypeid",
                        column: x => x.productpropertytypeid,
                        principalTable: "productpropertytype",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "district",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    locationid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_district", x => x.id);
                    table.ForeignKey(
                        name: "FK_district_location_locationid",
                        column: x => x.locationid,
                        principalTable: "location",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ward",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    districtid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ward", x => x.id);
                    table.ForeignKey(
                        name: "FK_ward_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productbrand",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productbrandcategoryid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandtypeid = table.Column<long>(type: "bigint", nullable: true),
                    departmentmanid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandmodelmanagement_id = table.Column<long>(type: "bigint", nullable: true),
                    productbrandlevelid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandstatusid = table.Column<long>(type: "bigint", nullable: true),
                    countryid = table.Column<long>(type: "bigint", nullable: true),
                    locationid = table.Column<long>(type: "bigint", nullable: true),
                    districtid = table.Column<long>(type: "bigint", nullable: true),
                    wardid = table.Column<long>(type: "bigint", nullable: true),
                    bankid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    qrcodepublic = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    tradingname = table.Column<string>(type: "text", nullable: true),
                    brandname = table.Column<string>(type: "text", nullable: true),
                    taxcode = table.Column<string>(type: "text", nullable: true),
                    registrationnumber = table.Column<string>(type: "text", nullable: true),
                    issueddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    businessarea = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    telephone = table.Column<string>(type: "text", nullable: true),
                    fax = table.Column<string>(type: "text", nullable: true),
                    mobile = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "text", nullable: true),
                    facebook = table.Column<string>(type: "text", nullable: true),
                    zalo = table.Column<string>(type: "text", nullable: true),
                    hotline = table.Column<string>(type: "text", nullable: true),
                    skype = table.Column<string>(type: "text", nullable: true),
                    bankacc = table.Column<string>(type: "text", nullable: true),
                    prinfo = table.Column<string>(type: "text", nullable: true),
                    agency = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    legaldocument = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    personsurname = table.Column<string>(type: "text", nullable: true),
                    personname = table.Column<string>(type: "text", nullable: true),
                    personaddress = table.Column<string>(type: "text", nullable: true),
                    personmobile = table.Column<string>(type: "text", nullable: true),
                    personzalo = table.Column<string>(type: "text", nullable: true),
                    personemail = table.Column<string>(type: "text", nullable: true),
                    personposition = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    hasqrcode = table.Column<bool>(type: "boolean", nullable: true),
                    viewcount = table.Column<int>(type: "integer", nullable: true),
                    viewpagecount = table.Column<int>(type: "integer", nullable: true),
                    followcount = table.Column<int>(type: "integer", nullable: true),
                    sellcount = table.Column<int>(type: "integer", nullable: true),
                    accountusername = table.Column<string>(type: "text", nullable: true),
                    accountemail = table.Column<string>(type: "text", nullable: true),
                    directorname = table.Column<string>(type: "text", nullable: true),
                    directorbirthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    directoraddress = table.Column<string>(type: "text", nullable: true),
                    directormobile = table.Column<string>(type: "text", nullable: true),
                    directoremail = table.Column<string>(type: "text", nullable: true),
                    directorposition = table.Column<string>(type: "text", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    @checked = table.Column<int>(name: "checked", type: "integer", nullable: true),
                    checkby = table.Column<string>(type: "text", nullable: true),
                    checkdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    approved = table.Column<int>(type: "integer", nullable: true),
                    approveby = table.Column<string>(type: "text", nullable: true),
                    approvedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    googlemapcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrand", x => x.id);
                    table.ForeignKey(
                        name: "FK_productbrand_bank_bankid",
                        column: x => x.bankid,
                        principalTable: "bank",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_location_locationid",
                        column: x => x.locationid,
                        principalTable: "location",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_productbrandcategory_productbrandcategoryid",
                        column: x => x.productbrandcategoryid,
                        principalTable: "productbrandcategory",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_productbrandlevel_productbrandlevelid",
                        column: x => x.productbrandlevelid,
                        principalTable: "productbrandlevel",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_productbrandstatus_productbrandstatusid",
                        column: x => x.productbrandstatusid,
                        principalTable: "productbrandstatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_productbrandtype_productbrandtypeid",
                        column: x => x.productbrandtypeid,
                        principalTable: "productbrandtype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productbrand_ward_wardid",
                        column: x => x.wardid,
                        principalTable: "ward",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articletypeid = table.Column<long>(type: "bigint", nullable: true),
                    articlestatusid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    articlecategoryids = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    subtitle = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    imagedescription = table.Column<string>(type: "text", nullable: true),
                    bannerimage = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    author = table.Column<string>(type: "text", nullable: true),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    @checked = table.Column<int>(name: "checked", type: "integer", nullable: true),
                    checkby = table.Column<string>(type: "text", nullable: true),
                    checkdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    approved = table.Column<int>(type: "integer", nullable: true),
                    approveby = table.Column<string>(type: "text", nullable: true),
                    approvedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: true),
                    cancopy = table.Column<bool>(type: "boolean", nullable: true),
                    cancomment = table.Column<bool>(type: "boolean", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true),
                    metatitle = table.Column<string>(type: "text", nullable: true),
                    metadescription = table.Column<string>(type: "text", nullable: true),
                    metakeywords = table.Column<string>(type: "text", nullable: true),
                    documentrefer = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => x.id);
                    table.ForeignKey(
                        name: "FK_article_articlestatus_articlestatusid",
                        column: x => x.articlestatusid,
                        principalTable: "articlestatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_article_articletype_articletypeid",
                        column: x => x.articletypeid,
                        principalTable: "articletype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_article_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_department_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    producttypeid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    productmanufactureid = table.Column<long>(type: "bigint", nullable: true),
                    productstatusid = table.Column<long>(type: "bigint", nullable: true),
                    countryid = table.Column<long>(type: "bigint", nullable: true),
                    unitid = table.Column<long>(type: "bigint", nullable: true),
                    productcategoryids = table.Column<string>(type: "text", nullable: true),
                    barcode = table.Column<string>(type: "text", nullable: true),
                    manufacturesku = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    qrcodepublic = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    subtitle = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    imagedescription = table.Column<string>(type: "text", nullable: true),
                    bannerimage = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    specification = table.Column<string>(type: "text", nullable: true),
                    productcertificate = table.Column<string>(type: "text", nullable: true),
                    legalinfo = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    priceold = table.Column<decimal>(type: "numeric", nullable: true),
                    pricewholesale = table.Column<decimal>(type: "numeric", nullable: true),
                    wholesalemin = table.Column<int>(type: "integer", nullable: true),
                    discount = table.Column<decimal>(type: "numeric", nullable: true),
                    discountrate = table.Column<int>(type: "integer", nullable: true),
                    issecondhand = table.Column<bool>(type: "boolean", nullable: true),
                    isauthor = table.Column<bool>(type: "boolean", nullable: true),
                    isbestsale = table.Column<bool>(type: "boolean", nullable: true),
                    issaleoff = table.Column<bool>(type: "boolean", nullable: true),
                    isnew = table.Column<bool>(type: "boolean", nullable: true),
                    iscomming = table.Column<bool>(type: "boolean", nullable: true),
                    isoutstock = table.Column<bool>(type: "boolean", nullable: true),
                    isdiscontinue = table.Column<bool>(type: "boolean", nullable: true),
                    amountdefault = table.Column<int>(type: "integer", nullable: true),
                    expirydisplay = table.Column<string>(type: "text", nullable: true),
                    expirybyday = table.Column<int>(type: "integer", nullable: true),
                    warrantydisplay = table.Column<string>(type: "text", nullable: true),
                    warrantybymonth = table.Column<int>(type: "integer", nullable: true),
                    rate = table.Column<int>(type: "integer", nullable: true),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    counter = table.Column<int>(type: "integer", nullable: true),
                    likecount = table.Column<int>(type: "integer", nullable: true),
                    sellcount = table.Column<int>(type: "integer", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    @checked = table.Column<int>(name: "checked", type: "integer", nullable: true),
                    checkby = table.Column<string>(type: "text", nullable: true),
                    checkdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    approved = table.Column<int>(type: "integer", nullable: true),
                    approveby = table.Column<string>(type: "text", nullable: true),
                    approvedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string>(type: "text", nullable: true),
                    cancopy = table.Column<bool>(type: "boolean", nullable: true),
                    cancomment = table.Column<bool>(type: "boolean", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true),
                    metatitle = table.Column<string>(type: "text", nullable: true),
                    metadescription = table.Column<string>(type: "text", nullable: true),
                    metakeywords = table.Column<string>(type: "text", nullable: true),
                    documentrefer = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_productmanufacture_productmanufactureid",
                        column: x => x.productmanufactureid,
                        principalTable: "productmanufacture",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_productstatus_productstatusid",
                        column: x => x.productstatusid,
                        principalTable: "productstatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_producttype_producttypeid",
                        column: x => x.producttypeid,
                        principalTable: "producttype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_unit_unitid",
                        column: x => x.unitid,
                        principalTable: "unit",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productbrandattachfile",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    attachfilename = table.Column<string>(type: "text", nullable: true),
                    filetype = table.Column<string>(type: "text", nullable: true),
                    filesize = table.Column<double>(type: "double precision", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrandattachfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_productbrandattachfile_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productbrandfollow",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    customerid = table.Column<long>(type: "bigint", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productbrandfollow", x => x.id);
                    table.ForeignKey(
                        name: "FK_productbrandfollow_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "articleattachfile",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articleid = table.Column<long>(type: "bigint", nullable: true),
                    attachfilename = table.Column<string>(type: "text", nullable: true),
                    filetype = table.Column<string>(type: "text", nullable: true),
                    filesize = table.Column<double>(type: "double precision", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articleattachfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_articleattachfile_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "articleblockarticle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articleblockid = table.Column<long>(type: "bigint", nullable: true),
                    articleid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articleblockarticle", x => x.id);
                    table.ForeignKey(
                        name: "FK_articleblockarticle_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_articleblockarticle_articleblock_articleblockid",
                        column: x => x.articleblockid,
                        principalTable: "articleblock",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "articlecategoryarticle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articlecategoryid = table.Column<long>(type: "bigint", nullable: true),
                    articleid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlecategoryarticle", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlecategoryarticle_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_articlecategoryarticle_articlecategory_articlecategoryid",
                        column: x => x.articlecategoryid,
                        principalTable: "articlecategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "articlecomment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articleid = table.Column<long>(type: "bigint", nullable: true),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlecomment", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlecomment_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_articlecomment_articlecomment_parentid",
                        column: x => x.parentid,
                        principalTable: "articlecomment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "articlecommentstaff",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articleid = table.Column<long>(type: "bigint", nullable: true),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlecommentstaff", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlecommentstaff_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "articlerelationarticle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articleid = table.Column<long>(type: "bigint", nullable: true),
                    articlerelationid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articlerelationarticle", x => x.id);
                    table.ForeignKey(
                        name: "FK_articlerelationarticle_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_articlerelationarticle_article_articlerelationid",
                        column: x => x.articlerelationid,
                        principalTable: "article",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "articletop",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    articlecategoryid = table.Column<long>(type: "bigint", nullable: true),
                    articleid = table.Column<long>(type: "bigint", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articletop", x => x.id);
                    table.ForeignKey(
                        name: "FK_articletop_article_articleid",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_articletop_articlecategory_articlecategoryid",
                        column: x => x.articlecategoryid,
                        principalTable: "articlecategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productattachfile",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    attachfilename = table.Column<string>(type: "text", nullable: true),
                    filetype = table.Column<string>(type: "text", nullable: true),
                    filesize = table.Column<double>(type: "double precision", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productattachfile", x => x.id);
                    table.ForeignKey(
                        name: "FK_productattachfile_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productblockproduct",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productblockid = table.Column<long>(type: "bigint", nullable: false),
                    productid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productblockproduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_productblockproduct_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productblockproduct_productblock_productblockid",
                        column: x => x.productblockid,
                        principalTable: "productblock",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productcategoryproduct",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productid = table.Column<long>(type: "bigint", nullable: false),
                    productcategoryid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcategoryproduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_productcategoryproduct_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productcategoryproduct_productcategory_productcategoryid",
                        column: x => x.productcategoryid,
                        principalTable: "productcategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productcomment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcomment", x => x.id);
                    table.ForeignKey(
                        name: "FK_productcomment_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productcomment_productcomment_parentid",
                        column: x => x.parentid,
                        principalTable: "productcomment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productcommentstaff",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcommentstaff", x => x.id);
                    table.ForeignKey(
                        name: "FK_productcommentstaff_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productlike",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    customerid = table.Column<long>(type: "bigint", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productlike", x => x.id);
                    table.ForeignKey(
                        name: "FK_productlike_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productlike_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productpicture",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    sort = table.Column<int>(type: "integer", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productpicture", x => x.id);
                    table.ForeignKey(
                        name: "FK_productpicture_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productpropertyvalue",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    productpropertyid = table.Column<long>(type: "bigint", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productpropertyvalue", x => x.id);
                    table.ForeignKey(
                        name: "FK_productpropertyvalue_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productpropertyvalue_productproperty_productpropertyid",
                        column: x => x.productpropertyid,
                        principalTable: "productproperty",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productrelationproduct",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    productrelationid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productrelationproduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_productrelationproduct_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_productrelationproduct_product_productrelationid",
                        column: x => x.productrelationid,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "productreview",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productreviewtypeid = table.Column<long>(type: "bigint", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    customerid = table.Column<long>(type: "bigint", nullable: true),
                    qrcodepubliccontent = table.Column<string>(type: "text", nullable: true),
                    qrcodesecretcontent = table.Column<string>(type: "text", nullable: true),
                    star = table.Column<int>(type: "integer", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productreview", x => x.id);
                    table.ForeignKey(
                        name: "FK_productreview_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productreview_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "producttop",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productcategoryid = table.Column<long>(type: "bigint", nullable: true),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producttop", x => x.id);
                    table.ForeignKey(
                        name: "FK_producttop_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_producttop_productcategory_productcategoryid",
                        column: x => x.productcategoryid,
                        principalTable: "productcategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserclaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "text", nullable: false),
                    claimtype = table.Column<string>(type: "text", nullable: true),
                    claimvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserclaims", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserlogins",
                columns: table => new
                {
                    loginprovider = table.Column<string>(type: "text", nullable: false),
                    providerkey = table.Column<string>(type: "text", nullable: false),
                    providerdisplayname = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserlogins", x => new { x.loginprovider, x.providerkey });
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserprofiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "text", nullable: true),
                    regtype = table.Column<string>(type: "text", nullable: true),
                    registerdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: true),
                    verifieddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastlogindate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastactivitydate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    fullname = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<bool>(type: "boolean", nullable: true),
                    birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    departmentid = table.Column<long>(type: "bigint", nullable: true),
                    rank = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    countryid = table.Column<long>(type: "bigint", nullable: true),
                    locationid = table.Column<long>(type: "bigint", nullable: true),
                    districtid = table.Column<long>(type: "bigint", nullable: true),
                    wardid = table.Column<long>(type: "bigint", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    website = table.Column<string>(type: "text", nullable: true),
                    facebookid = table.Column<string>(type: "text", nullable: true),
                    skype = table.Column<string>(type: "text", nullable: true),
                    zalo = table.Column<string>(type: "text", nullable: true),
                    telegram = table.Column<string>(type: "text", nullable: true),
                    avatarurl = table.Column<string>(type: "text", nullable: true),
                    signature = table.Column<string>(type: "text", nullable: true),
                    accounttype = table.Column<int>(type: "integer", nullable: true),
                    department = table.Column<string>(type: "text", nullable: true),
                    allownotifyapp = table.Column<bool>(type: "boolean", nullable: true),
                    allownotifyemail = table.Column<bool>(type: "boolean", nullable: true),
                    allownotifysms = table.Column<bool>(type: "boolean", nullable: true),
                    bankacc = table.Column<string>(type: "text", nullable: true),
                    bankid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserprofiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_aspnetuserprofiles_bank_bankid",
                        column: x => x.bankid,
                        principalTable: "bank",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aspnetuserprofiles_country_countryid",
                        column: x => x.countryid,
                        principalTable: "country",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aspnetuserprofiles_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aspnetuserprofiles_location_locationid",
                        column: x => x.locationid,
                        principalTable: "location",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aspnetuserprofiles_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aspnetuserprofiles_ward_wardid",
                        column: x => x.wardid,
                        principalTable: "ward",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "aspnetusers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    emailconfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    phonenumberconfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    twofactorenabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockoutenabled = table.Column<bool>(type: "boolean", nullable: false),
                    accessfailedcount = table.Column<int>(type: "integer", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: true),
                    username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    passwordhash = table.Column<string>(type: "text", nullable: true),
                    securitystamp = table.Column<string>(type: "text", nullable: true),
                    concurrencystamp = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    lockoutend = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetusers", x => x.id);
                    table.ForeignKey(
                        name: "FK_aspnetusers_aspnetuserprofiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "aspnetuserprofiles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserroles",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    roleid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserroles", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "FK_aspnetuserroles_aspnetroles_roleid",
                        column: x => x.roleid,
                        principalTable: "aspnetroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aspnetuserroles_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetusertokens",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    loginprovider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetusertokens", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "FK_aspnetusertokens_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productorder",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ordercode = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<string>(type: "text", nullable: true),
                    productbrandid = table.Column<long>(type: "bigint", nullable: true),
                    productorderstatusid = table.Column<long>(type: "bigint", nullable: true),
                    productorderpaymentmethodid = table.Column<long>(type: "bigint", nullable: true),
                    productorderpaymentstatusid = table.Column<long>(type: "bigint", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    customeremail = table.Column<string>(type: "text", nullable: true),
                    customerphone = table.Column<string>(type: "text", nullable: true),
                    customeraddress = table.Column<string>(type: "text", nullable: true),
                    locationid = table.Column<long>(type: "bigint", nullable: true),
                    districtid = table.Column<long>(type: "bigint", nullable: true),
                    wardid = table.Column<long>(type: "bigint", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    shippingfee = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    discount = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    tax = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    total = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    couponcode = table.Column<string>(type: "text", nullable: true),
                    transactionid = table.Column<string>(type: "text", nullable: true),
                    paymentdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    shippingdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deliverydate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    canceldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelreason = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lasteditby = table.Column<string>(type: "text", nullable: true),
                    lasteditdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productorder", x => x.id);
                    table.ForeignKey(
                        name: "FK_productorder_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_district_districtid",
                        column: x => x.districtid,
                        principalTable: "district",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_location_locationid",
                        column: x => x.locationid,
                        principalTable: "location",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_productbrand_productbrandid",
                        column: x => x.productbrandid,
                        principalTable: "productbrand",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_productorderpaymentmethod_productorderpaymentm~",
                        column: x => x.productorderpaymentmethodid,
                        principalTable: "productorderpaymentmethod",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_productorderpaymentstatus_productorderpayments~",
                        column: x => x.productorderpaymentstatusid,
                        principalTable: "productorderpaymentstatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_productorderstatus_productorderstatusid",
                        column: x => x.productorderstatusid,
                        principalTable: "productorderstatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorder_ward_wardid",
                        column: x => x.wardid,
                        principalTable: "ward",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "refreshtoken",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "text", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    jwtid = table.Column<string>(type: "text", nullable: false),
                    isused = table.Column<bool>(type: "boolean", nullable: false),
                    isrevoked = table.Column<bool>(type: "boolean", nullable: false),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expirydate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshtoken", x => x.id);
                    table.ForeignKey(
                        name: "FK_refreshtoken_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usernotify",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "text", nullable: true),
                    notificationtypeid = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    isread = table.Column<bool>(type: "boolean", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usernotify", x => x.id);
                    table.ForeignKey(
                        name: "FK_usernotify_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productorderdetail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productorderid = table.Column<long>(type: "bigint", nullable: true),
                    productid = table.Column<long>(type: "bigint", nullable: true),
                    productname = table.Column<string>(type: "text", nullable: true),
                    productimage = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    discount = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    total = table.Column<decimal>(type: "numeric(19,4)", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    createby = table.Column<string>(type: "text", nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productorderdetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_productorderdetail_product_productid",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productorderdetail_productorder_productorderid",
                        column: x => x.productorderid,
                        principalTable: "productorder",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_advertising_advertisingblockid",
                table: "advertising",
                column: "advertisingblockid");

            migrationBuilder.CreateIndex(
                name: "IX_advertising_advertisingtypeid",
                table: "advertising",
                column: "advertisingtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_article_active",
                table: "article",
                column: "active");

            migrationBuilder.CreateIndex(
                name: "IX_article_articlestatusid",
                table: "article",
                column: "articlestatusid");

            migrationBuilder.CreateIndex(
                name: "IX_article_articletypeid",
                table: "article",
                column: "articletypeid");

            migrationBuilder.CreateIndex(
                name: "IX_article_createdate",
                table: "article",
                column: "createdate");

            migrationBuilder.CreateIndex(
                name: "IX_article_productbrandid",
                table: "article",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_article_url",
                table: "article",
                column: "url");

            migrationBuilder.CreateIndex(
                name: "IX_articleattachfile_articleid",
                table: "articleattachfile",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articleblockarticle_articleblockid",
                table: "articleblockarticle",
                column: "articleblockid");

            migrationBuilder.CreateIndex(
                name: "IX_articleblockarticle_articleid",
                table: "articleblockarticle",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecategory_parentid",
                table: "articlecategory",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecategoryarticle_articlecategoryid",
                table: "articlecategoryarticle",
                column: "articlecategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecategoryarticle_articleid",
                table: "articlecategoryarticle",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecategoryassign_articlecategoryid",
                table: "articlecategoryassign",
                column: "articlecategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecomment_articleid",
                table: "articlecomment",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecomment_parentid",
                table: "articlecomment",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_articlecommentstaff_articleid",
                table: "articlecommentstaff",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articlerelationarticle_articleid",
                table: "articlerelationarticle",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_articlerelationarticle_articlerelationid",
                table: "articlerelationarticle",
                column: "articlerelationid");

            migrationBuilder.CreateIndex(
                name: "IX_articletop_articlecategoryid",
                table: "articletop",
                column: "articlecategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_articletop_articleid",
                table: "articletop",
                column: "articleid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetroleclaims_roleid",
                table: "aspnetroleclaims",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "aspnetroles",
                column: "normalizedname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserclaims_userid",
                table: "aspnetuserclaims",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserlogins_userid",
                table: "aspnetuserlogins",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_bankid",
                table: "aspnetuserprofiles",
                column: "bankid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_countryid",
                table: "aspnetuserprofiles",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_districtid",
                table: "aspnetuserprofiles",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_locationid",
                table: "aspnetuserprofiles",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_productbrandid",
                table: "aspnetuserprofiles",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_userid",
                table: "aspnetuserprofiles",
                column: "userid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserprofiles_wardid",
                table: "aspnetuserprofiles",
                column: "wardid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserroles_roleid",
                table: "aspnetuserroles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "aspnetusers",
                column: "normalizedemail");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetusers_ProfileId",
                table: "aspnetusers",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "aspnetusers",
                column: "normalizedusername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_productbrandid",
                table: "department",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_departmentman_parentid",
                table: "departmentman",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_district_locationid",
                table: "district",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_location_countryid",
                table: "location",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_product_active",
                table: "product",
                column: "active");

            migrationBuilder.CreateIndex(
                name: "IX_product_countryid",
                table: "product",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_product_createdate",
                table: "product",
                column: "createdate");

            migrationBuilder.CreateIndex(
                name: "IX_product_productbrandid",
                table: "product",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_product_productmanufactureid",
                table: "product",
                column: "productmanufactureid");

            migrationBuilder.CreateIndex(
                name: "IX_product_productstatusid",
                table: "product",
                column: "productstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_product_producttypeid",
                table: "product",
                column: "producttypeid");

            migrationBuilder.CreateIndex(
                name: "IX_product_sku",
                table: "product",
                column: "sku");

            migrationBuilder.CreateIndex(
                name: "IX_product_unitid",
                table: "product",
                column: "unitid");

            migrationBuilder.CreateIndex(
                name: "IX_product_url",
                table: "product",
                column: "url");

            migrationBuilder.CreateIndex(
                name: "IX_productattachfile_productid",
                table: "productattachfile",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productblockproduct_productblockid",
                table: "productblockproduct",
                column: "productblockid");

            migrationBuilder.CreateIndex(
                name: "IX_productblockproduct_productid",
                table: "productblockproduct",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_bankid",
                table: "productbrand",
                column: "bankid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_countryid",
                table: "productbrand",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_districtid",
                table: "productbrand",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_locationid",
                table: "productbrand",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_productbrandcategoryid",
                table: "productbrand",
                column: "productbrandcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_productbrandlevelid",
                table: "productbrand",
                column: "productbrandlevelid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_productbrandstatusid",
                table: "productbrand",
                column: "productbrandstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_productbrandtypeid",
                table: "productbrand",
                column: "productbrandtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrand_wardid",
                table: "productbrand",
                column: "wardid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrandattachfile_productbrandid",
                table: "productbrandattachfile",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrandcategory_parentid",
                table: "productbrandcategory",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_productbrandfollow_productbrandid",
                table: "productbrandfollow",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_productcategory_parentid",
                table: "productcategory",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_productcategoryassign_productcategoryid",
                table: "productcategoryassign",
                column: "productcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_productcategoryproduct_productcategoryid",
                table: "productcategoryproduct",
                column: "productcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_productcategoryproduct_productid",
                table: "productcategoryproduct",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productcomment_parentid",
                table: "productcomment",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_productcomment_productid",
                table: "productcomment",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productcommentstaff_productid",
                table: "productcommentstaff",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productlike_productbrandid",
                table: "productlike",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_productlike_productid",
                table: "productlike",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_districtid",
                table: "productorder",
                column: "districtid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_locationid",
                table: "productorder",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_ordercode",
                table: "productorder",
                column: "ordercode");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_productbrandid",
                table: "productorder",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_productorderpaymentmethodid",
                table: "productorder",
                column: "productorderpaymentmethodid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_productorderpaymentstatusid",
                table: "productorder",
                column: "productorderpaymentstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_productorderstatusid",
                table: "productorder",
                column: "productorderstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_userid",
                table: "productorder",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_productorder_wardid",
                table: "productorder",
                column: "wardid");

            migrationBuilder.CreateIndex(
                name: "IX_productorderdetail_productid",
                table: "productorderdetail",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productorderdetail_productorderid",
                table: "productorderdetail",
                column: "productorderid");

            migrationBuilder.CreateIndex(
                name: "IX_productpicture_productid",
                table: "productpicture",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productproperty_productpropertycategoryid",
                table: "productproperty",
                column: "productpropertycategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_productproperty_productpropertytypeid",
                table: "productproperty",
                column: "productpropertytypeid");

            migrationBuilder.CreateIndex(
                name: "IX_productpropertyvalue_productid",
                table: "productpropertyvalue",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productpropertyvalue_productpropertyid",
                table: "productpropertyvalue",
                column: "productpropertyid");

            migrationBuilder.CreateIndex(
                name: "IX_productrelationproduct_productid",
                table: "productrelationproduct",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productrelationproduct_productrelationid",
                table: "productrelationproduct",
                column: "productrelationid");

            migrationBuilder.CreateIndex(
                name: "IX_productreview_productbrandid",
                table: "productreview",
                column: "productbrandid");

            migrationBuilder.CreateIndex(
                name: "IX_productreview_productid",
                table: "productreview",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_producttop_productcategoryid",
                table: "producttop",
                column: "productcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_producttop_productid",
                table: "producttop",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_refreshtoken_token",
                table: "refreshtoken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refreshtoken_userid",
                table: "refreshtoken",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_usernotify_userid",
                table: "usernotify",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_ward_districtid",
                table: "ward",
                column: "districtid");

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserclaims_aspnetusers_userid",
                table: "aspnetuserclaims",
                column: "userid",
                principalTable: "aspnetusers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserlogins_aspnetusers_userid",
                table: "aspnetuserlogins",
                column: "userid",
                principalTable: "aspnetusers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetuserprofiles_aspnetusers_userid",
                table: "aspnetuserprofiles",
                column: "userid",
                principalTable: "aspnetusers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aspnetuserprofiles_productbrand_productbrandid",
                table: "aspnetuserprofiles");

            migrationBuilder.DropForeignKey(
                name: "FK_aspnetuserprofiles_aspnetusers_userid",
                table: "aspnetuserprofiles");

            migrationBuilder.DropTable(
                name: "advertising");

            migrationBuilder.DropTable(
                name: "advise");

            migrationBuilder.DropTable(
                name: "articleattachfile");

            migrationBuilder.DropTable(
                name: "articleblockarticle");

            migrationBuilder.DropTable(
                name: "articlecategoryarticle");

            migrationBuilder.DropTable(
                name: "articlecategoryassign");

            migrationBuilder.DropTable(
                name: "articlecomment");

            migrationBuilder.DropTable(
                name: "articlecommentstaff");

            migrationBuilder.DropTable(
                name: "articlerelationarticle");

            migrationBuilder.DropTable(
                name: "articletop");

            migrationBuilder.DropTable(
                name: "aspnetroleclaims");

            migrationBuilder.DropTable(
                name: "aspnetuserclaims");

            migrationBuilder.DropTable(
                name: "aspnetuserlogins");

            migrationBuilder.DropTable(
                name: "aspnetuserroles");

            migrationBuilder.DropTable(
                name: "aspnetusertokens");

            migrationBuilder.DropTable(
                name: "contact");

            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "departmentman");

            migrationBuilder.DropTable(
                name: "logvisit");

            migrationBuilder.DropTable(
                name: "newsletter_subscription");

            migrationBuilder.DropTable(
                name: "productattachfile");

            migrationBuilder.DropTable(
                name: "productblockproduct");

            migrationBuilder.DropTable(
                name: "productbrandattachfile");

            migrationBuilder.DropTable(
                name: "productbrandfollow");

            migrationBuilder.DropTable(
                name: "productcategoryassign");

            migrationBuilder.DropTable(
                name: "productcategoryproduct");

            migrationBuilder.DropTable(
                name: "productcomment");

            migrationBuilder.DropTable(
                name: "productcommentstaff");

            migrationBuilder.DropTable(
                name: "productlike");

            migrationBuilder.DropTable(
                name: "productorderdetail");

            migrationBuilder.DropTable(
                name: "productpicture");

            migrationBuilder.DropTable(
                name: "productpropertyvalue");

            migrationBuilder.DropTable(
                name: "productrelationproduct");

            migrationBuilder.DropTable(
                name: "productreview");

            migrationBuilder.DropTable(
                name: "producttop");

            migrationBuilder.DropTable(
                name: "refreshtoken");

            migrationBuilder.DropTable(
                name: "replacechar");

            migrationBuilder.DropTable(
                name: "setting");

            migrationBuilder.DropTable(
                name: "sysdiagrams");

            migrationBuilder.DropTable(
                name: "usernotify");

            migrationBuilder.DropTable(
                name: "advertisingblock");

            migrationBuilder.DropTable(
                name: "advertisingtype");

            migrationBuilder.DropTable(
                name: "articleblock");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "articlecategory");

            migrationBuilder.DropTable(
                name: "aspnetroles");

            migrationBuilder.DropTable(
                name: "productblock");

            migrationBuilder.DropTable(
                name: "productorder");

            migrationBuilder.DropTable(
                name: "productproperty");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "productcategory");

            migrationBuilder.DropTable(
                name: "articlestatus");

            migrationBuilder.DropTable(
                name: "articletype");

            migrationBuilder.DropTable(
                name: "productorderpaymentmethod");

            migrationBuilder.DropTable(
                name: "productorderpaymentstatus");

            migrationBuilder.DropTable(
                name: "productorderstatus");

            migrationBuilder.DropTable(
                name: "productpropertycategory");

            migrationBuilder.DropTable(
                name: "productpropertytype");

            migrationBuilder.DropTable(
                name: "productmanufacture");

            migrationBuilder.DropTable(
                name: "productstatus");

            migrationBuilder.DropTable(
                name: "producttype");

            migrationBuilder.DropTable(
                name: "unit");

            migrationBuilder.DropTable(
                name: "productbrand");

            migrationBuilder.DropTable(
                name: "productbrandcategory");

            migrationBuilder.DropTable(
                name: "productbrandlevel");

            migrationBuilder.DropTable(
                name: "productbrandstatus");

            migrationBuilder.DropTable(
                name: "productbrandtype");

            migrationBuilder.DropTable(
                name: "aspnetusers");

            migrationBuilder.DropTable(
                name: "aspnetuserprofiles");

            migrationBuilder.DropTable(
                name: "bank");

            migrationBuilder.DropTable(
                name: "ward");

            migrationBuilder.DropTable(
                name: "district");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
