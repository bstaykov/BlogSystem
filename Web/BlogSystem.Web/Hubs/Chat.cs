namespace BlogSystem.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.SignalR;
    using System.Threading.Tasks;
    using BlogSystem.Web.Hubs.ChatHelpersClasses;

    public class Chat : Hub
    {
        public void SendMessage(string message)
        {
            var userName = Context.User.Identity.Name;

            var msg = string.Format("{0}: {1}", userName, message);
            Clients.All.addMessage(msg);
        }

        public void SendAnonymousMessage(string message, string user)
        {
            var msg = string.Format("Anonymous - {0}: {1}", user, message);
            Clients.All.addMessage(msg);
        }

        public void JoinRoom(string room)
        {
            Groups.Add(Context.ConnectionId, room);
            Clients.Caller.joinRoom(room);
        }

        public void SendMessageToRoom(string message, string[] rooms)
        {
            var msg = string.Format("{0}: {1}", Context.User.Identity.Name, message);

            for (int i = 0; i < rooms.Length; i++)
            {
                Clients.Group(rooms[i]).addMessage(msg);
            }
        }

        public override Task OnConnected()
        {
            AddUserToCount();

            UpdateOnlineUsersCount();

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            AddUserToCount();

            UpdateOnlineUsersCount();

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            RemoveUserFromCount();

            UpdateOnlineUsersCount();

            return base.OnDisconnected(stopCalled);
        }

        private void AddUserToCount()
        {
            var userName = Context.User.Identity.Name;

            if (userName != string.Empty)
            {
                if (!(UserHandler.ConectedUsers.ContainsKey(userName)))
                {
                    UserHandler.ConectedUsers[userName] = 0;
                }

                UserHandler.ConectedUsers[userName] += 1;
            }
        }

        private void RemoveUserFromCount()
        {
            var userName = Context.User.Identity.Name;

            bool isUserLoged = userName != string.Empty;

            bool isUserInTheList = UserHandler.ConectedUsers.ContainsKey(userName);

            if (isUserLoged && isUserInTheList)
            {
                bool areMoreThanOneTabsOpened = UserHandler.ConectedUsers[userName] > 1;

                if (areMoreThanOneTabsOpened)
                {
                    UserHandler.ConectedUsers[userName] -= 1;
                }
                else
                {
                    UserHandler.ConectedUsers.Remove(userName);
                }
            }
        }

        public void RemoveUserOnLogOut()
        {
            var userName = Context.User.Identity.Name;

            bool isUserLoged = userName != string.Empty;

            bool isUserInTheList = UserHandler.ConectedUsers.ContainsKey(userName);

            if (isUserLoged && isUserInTheList)
            {
                UserHandler.ConectedUsers.Remove(userName);
            }

            UpdateOnlineUsersCount();
        }

        public void UpdateOnlineUsersCount()
        {
            Clients.All.UpdateOnlineUsersCount(UserHandler.ConectedUsers.Count);
        }

        public void GetOnlineUsers()
        {
            string[] onlineUsers = UserHandler.ConectedUsers.Keys.ToArray();

            Clients.Caller.ListOfUsersOnline(onlineUsers);
        }

        //public void AddToUserCount()
        //{
        //    var userName = Context.User.Identity.Name;

        //    var isUserLoged = userName != string.Empty;
        //    var isUserCounted = !(UserHandler.ConectedUsers.ContainsKey(userName));

        //    if (isUserLoged && isUserCounted)
        //    {
        //        UserHandler.ConnectedUsers.Add(userName);

        //        SendOnlineUsersCount();
        //    }
        //}

        //public void RemoveFromUserCount()
        //{
        //    var userName = Context.User.Identity.Name;

        //    if (userName != string.Empty && UserHandler.ConnectedUsers.Contains(userName))
        //    {
        //        UserHandler.ConnectedUsers.Remove(userName);
        //        SendOnlineUsersCount();
        //    }
        //}
    }

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
        public static Dictionary<string, int> ConectedUsers = new Dictionary<string, int>();
        //public static HashSet<string> ConnectedUsers = new HashSet<string>();

        public static PublicChatRoom publicChatRoom = new PublicChatRoom();
    }
}