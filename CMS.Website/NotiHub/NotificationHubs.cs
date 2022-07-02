using Microsoft.AspNetCore.SignalR;

namespace CMS.Website.NotiHub
{

    public class NotificationHubs : Hub
    {
        private RepositoryWrapper Repository;
        private static List<ConnectedUser> connectedUsers = new();
        public NotificationHubs(RepositoryWrapper _repository)
        {
            Repository = _repository;
        }
        public async Task SendAllNotification(string userId, string subject)
        {
            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                if ((bool)profile.AllowNotifyApp)
                {
                    await Clients.All.SendAsync("ReceiveMessage", userId, subject);
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, subject);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }
        //Sent Msg Back To The Caller
        public async Task SendNotificationToCaller(string userId, string subject, string content, string url, string imageUrl)
        {


            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                //save in user noti
                var model = new UserNotify();
                model.AspNetUsersId = userId;
                model.Subject = subject;
                model.Content = content;
                model.Url = url;
                model.ImageUrl = imageUrl;
                await Repository.UserNoti.UserNotiCreateNew(model);
                if ((bool)profile.AllowNotifyApp)
                {
                    try
                    {
                        await Clients.Caller.SendAsync("ReceiveMessage", userId, subject, content, url, imageUrl);


                    }
                    catch
                    {

                    }
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, content);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }
        public async Task SendNotification(string userId, string subject, string content, string url, string imageUrl)
        {
            //var userIdentifier = (from _connectedUser in connectedUsers
            //                      where _connectedUser.UserIdentifier == userId
            //                      select _connectedUser.UserIdentifier).FirstOrDefault();
            var lstConnectionIds = (from _connectedUser in connectedUsers
                                    where _connectedUser.UserIdentifier == userId
                                    select _connectedUser.Connections).FirstOrDefault();

            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                //save in user noti
                var model = new UserNotify();
                model.AspNetUsersId = userId;
                model.Subject = subject;
                model.Content = content;
                model.Url = url;
                model.ImageUrl = imageUrl;
                await Repository.UserNoti.UserNotiCreateNew(model);
                if ((bool)profile.AllowNotifyApp)
                {
                    try
                    {
                        if (lstConnectionIds != null)
                        {
                            
                            foreach (var p in lstConnectionIds)
                            {                                
                                //await Clients.Clients(p.ConnectionID).SendAsync("ReceiveMessage", userId, subject, content, url, imageUrl);
                                await Clients.User(userId).SendAsync("ReceiveMessage", userId, subject, content, url, imageUrl);
                            }

                        }

                    }
                    catch
                    {

                    }
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, content);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        //        public override async Task OnDisconnectedAsync(Exception exception)
        //#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        //        {

        //            var user = connectedUsers.Where(cu => cu.UserIdentifier == Context.UserIdentifier).FirstOrDefault();
        //            if (user == null) return;
        //            var connection = user.Connections.Where(c => c.ConnectionID == Context.ConnectionId).FirstOrDefault();
        //            var count = user.Connections.Count;

        //            if (count == 1) // A single connection: remove user
        //            {
        //                connectedUsers.Remove(user);

        //            }
        //            if (count > 1) // Multiple connection: Remove current connection
        //            {
        //                user.Connections.Remove(connection);
        //            }


        //            //await Clients.All.SendAsync("ReceiveInitializeUserList", list);

        //            //await Clients.All.SendAsync("MessageBoard",
        //            //           $"{Context.User.Identity.Name}  has left");



        //        }
        public async Task SendGroupNotification(string userId, string subject)
        {
            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                if ((bool)profile.AllowNotifyApp)
                {
                    await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", userId, subject);
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, subject);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public override async Task OnConnectedAsync()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var httpCtx = Context.GetHttpContext();
            var connectedUserId = httpCtx.Request.Headers["USERID"].ToString();
            var connectedUserName = httpCtx.Request.Headers["USERNAME"].ToString();
            var user = connectedUsers.Where(cu => cu.UserIdentifier == connectedUserId).FirstOrDefault();

            if (user == null) // User does not exist
            {
                ConnectedUser connectedUser = new ConnectedUser
                {
                    UserIdentifier = connectedUserId,
                    Name = connectedUserName,
                    Connections = new List<Connection> { new Connection { ConnectionID = Context.ConnectionId } }
                };

                connectedUsers.Add(connectedUser);
            }
            //else
            //{
            //    user.Connections.Add(new Connection
            //    {
            //        ConnectionID = Context.ConnectionId
            //    });
            //}


        }
    }


    public class ConnectedUser
    {
        public string Name { get; set; }
        public string UserIdentifier { get; set; }

        public List<Connection> Connections { get; set; }
    }
    public class Connection
    {
        public string ConnectionID { get; set; }

    }
}

