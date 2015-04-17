namespace BlogSystem.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using BlogSystem.Web.Hubs.ChatHelpersClasses;

    using Microsoft.AspNet.SignalR;

    public class Chat : Hub
    {
        private static PublicChatRoom publicChatRoom = new PublicChatRoom();
        private static HashSet<string> connectedIds = new HashSet<string>();
        private static Dictionary<string, int> conectedUsers = new Dictionary<string, int>();

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
            this.AddUserToCount();

            this.UpdateOnlineUsersCount();

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            this.AddUserToCount();

            this.UpdateOnlineUsersCount();

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            this.RemoveUserFromCount();

            this.UpdateOnlineUsersCount();

            return base.OnDisconnected(stopCalled);
        }

        public void RemoveUserOnLogOut()
        {
            var userName = Context.User.Identity.Name;

            bool isUserLoged = userName != string.Empty;

            bool isUserInTheList = Chat.conectedUsers.ContainsKey(userName);

            if (isUserLoged && isUserInTheList)
            {
                Chat.conectedUsers.Remove(userName);
            }

            this.UpdateOnlineUsersCount();
        }

        public void UpdateOnlineUsersCount()
        {
            Clients.All.UpdateOnlineUsersCount(Chat.conectedUsers.Count);
        }

        public void GetOnlineUsers()
        {
            string[] onlineUsers = Chat.conectedUsers.Keys.ToArray();

            Clients.Caller.ListOfUsersOnline(onlineUsers);
        }

        // public void AddToUserCount()
        // {
        // var userName = Context.User.Identity.Name;
        // var isUserLoged = userName != string.Empty;
        // var isUserCounted = !(UserHandler.ConectedUsers.ContainsKey(userName));
        // if (isUserLoged && isUserCounted)
        // {
        // UserHandler.ConnectedUsers.Add(userName);
        // SendOnlineUsersCount();
        // }
        // }
        // public void RemoveFromUserCount()
        // {
        // var userName = Context.User.Identity.Name;
        // if (userName != string.Empty && UserHandler.ConnectedUsers.Contains(userName))
        // {
        // UserHandler.ConnectedUsers.Remove(userName);
        // SendOnlineUsersCount();
        // }
        // }
        private void RemoveUserFromCount()
        {
            var userName = Context.User.Identity.Name;

            bool isUserLoged = userName != string.Empty;

            bool isUserInTheList = Chat.conectedUsers.ContainsKey(userName);

            if (isUserLoged && isUserInTheList)
            {
                bool areMoreThanOneTabsOpened = Chat.conectedUsers[userName] > 1;

                if (areMoreThanOneTabsOpened)
                {
                    Chat.conectedUsers[userName] -= 1;
                }
                else
                {
                    Chat.conectedUsers.Remove(userName);
                }
            }
        }

        private void AddUserToCount()
        {
            var userName = Context.User.Identity.Name;

            if (userName != string.Empty)
            {
                if (!Chat.conectedUsers.ContainsKey(userName))
                {
                    Chat.conectedUsers[userName] = 0;
                }

                Chat.conectedUsers[userName] += 1;
            }
        }
    }
}