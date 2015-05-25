namespace BlogSystem.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using BlogSystem.Data;
    using BlogSystem.Models;
    using BlogSystem.Web.Hubs.ChatHelpersClasses;
    using BlogSystem.Web.Hubs.ViewModels;

    using Microsoft.AspNet.SignalR;

    public class Chat : Hub
    {
        private static PublicChatRoom publicChatRoom = new PublicChatRoom();
        private static Dictionary<string, int> conectedUsers = new Dictionary<string, int>();
        private static Dictionary<string, string> connectedIds = new Dictionary<string, string>();
        private readonly IBlogSystemData data;

        public Chat()
            : this(new BlogSystemData(new BlogSystemDbContext()))
        {
        }

        public Chat(IBlogSystemData data)
        {
            this.data = data;
        } 

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
            this.AddConnectionId();

            this.AddUserToCount();

            this.UpdateOnlineUsersCount();

            this.GetNewCommentsCount();

            this.UpdateUnreadMessagesCounter();

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            this.AddConnectionId();

            this.AddUserToCount();

            this.UpdateOnlineUsersCount();

            this.GetNewCommentsCount();

            this.UpdateUnreadMessagesCounter();

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            this.RemoveConnectionId();

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

        public void GetNewCommentsCount()
        {
            var userName = Context.User.Identity.Name;
            var commentsCount = this.data.Comments.All().Where(comment => comment.IsReadByAuthor == false && comment.IsDeleted == false && comment.User.UserName != userName && comment.Post.User.UserName == userName).Count();
            Clients.Caller.UpdateCommentsCounter(commentsCount);
        }

        public void UpdateUnreadMessagesCounter()
        {
            var userName = Context.User.Identity.Name;
            var unreadMessages = this.data.Messages.All()
                .Where(message => message.DialogId.Value != null 
                    && message.Receiver.UserName == userName 
                    && message.IsRead == false)
                .Count();
            Clients.Caller.updateUnreadMessagesCounter(unreadMessages);
        }

        public void GetListOfComments()
        {
            var userName = Context.User.Identity.Name;
            var comments = this.data.Comments.All().Where(comment => comment.User.UserName != userName && comment.Post.User.UserName == userName && comment.IsDeleted == false)
                .Project().To<LastCommentViewModel>()
                .OrderBy(comment => comment.CreatedOn)

                // .Skip((pageNumber - 1) * commentsPerPage)
                .Take(10)
                .ToList();
            Clients.Caller.DisplayListOfComments(comments);
        }

        public void RefreshUnreadMessagesCount(string userName)
        {
            if (connectedIds.ContainsKey(userName) == true)
            {
                var unreadMessages = this.data.Messages.All()
                .Where(message => message.DialogId.Value != null
                    && message.Receiver.UserName == userName
                    && message.IsRead == false)
                .Count();
                Clients.Client(connectedIds[userName]).updateUnreadMessagesCounter(unreadMessages);
            }
        }

        public void RefreshCommentsCount(int postId)
        {
            var userName = this.data.Posts.GetById(postId).User.UserName;

            if (connectedIds.ContainsKey(userName) == true)
            {
                var commentsCount = this.data.Comments.All().Where(comment => comment.IsReadByAuthor == false && comment.IsDeleted == false && comment.User.UserName != userName && comment.Post.User.UserName == userName).Count();
                Clients.Client(connectedIds[userName]).UpdateCommentsCounter(commentsCount);
            }
        }

        public void SendNewPostAlert()
        {
            var lastPost = this.data.Posts.All()
                .OrderByDescending(post => post.CreatedOn)
                .First();
            Clients.AllExcept(connectedIds[lastPost.User.UserName]).DisplayNewPostMessage(lastPost.User.UserName, lastPost.Id, lastPost.Title.Substring(0, 5) + "...");
        }

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

        private void AddConnectionId()
        {
            if (Context.User.Identity.IsAuthenticated == false)
            {
                return;
            }

            var userName = Context.User.Identity.Name;

            if (userName != null && connectedIds.ContainsKey(userName) == false)
            {
                connectedIds[userName] = Context.ConnectionId;
            }
        }

        private void RemoveConnectionId()
        {
            var userName = Context.User.Identity.Name;

            if (userName != null && connectedIds.ContainsKey(userName) == true)
            {
                connectedIds.Remove(userName);
            }
        }
    }
}