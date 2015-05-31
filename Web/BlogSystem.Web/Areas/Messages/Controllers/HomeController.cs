namespace BlogSystem.Web.Areas.Messages.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using BlogSystem.Models;
    using BlogSystem.Web.Areas.Messages.Models;
    using BlogSystem.Web.Controllers;

    using Microsoft.AspNet.Identity;

    [Authorize]
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult SendMessage(string userName)
        {
            var model = new MessageInputModel()
            {
                UserName = userName,
            };

            return this.PartialView("_SendMessage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(MessageInputModel model)
        {
            var userName = this.User.Identity.Name;
            var userId = this.User.Identity.GetUserId();

            if (userName == model.UserName)
            {
                ModelState.AddModelError(string.Empty, "Can not send message to yourself!");
            }

            var receiver = this.Data.Users.All().FirstOrDefault(user => user.UserName == model.UserName);

            if (receiver == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid message receiver!!!");
            }

            if (ModelState.IsValid == false)
            {
                ModelState.AddModelError(string.Empty, "Invalid model state!");
                return this.PartialView("_SendMessage", model);
            }

            var existingDialog = this.Data.Messages.All()
                .FirstOrDefault(message =>
                    (message.SenderId == userId && message.ReceiverId == receiver.Id)
                        || (message.SenderId == receiver.Id && message.ReceiverId == userId));

            if (existingDialog == null)
            {
                existingDialog = new Message()
                {
                    SenderId = userId,
                    ReceiverId = receiver.Id,
                    Content = "Dialog Started",
                    SendOn = DateTime.Now,
                    IsRead = false,
                };
                this.Data.Messages.Add(existingDialog);
                this.Data.Messages.SaveChanges();
            }

            var newMessage = new Message()
            {
                DialogId = existingDialog.Id,
                SenderId = userId,
                ReceiverId = receiver.Id,
                SendOn = DateTime.Now,
                Content = model.Content,
                IsRead = false,
            };
            this.Data.Messages.Add(newMessage);
            this.Data.Messages.SaveChanges();

            return this.PartialView("_SendMessageLink", model.UserName);
        }

        [HttpGet]
        public ActionResult GetMessages(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var userId = this.User.Identity.GetUserId();

            var messages = this.Data.Messages.All()
                .Where(message => message.DialogId.Value != null && (message.SenderId == userId || message.ReceiverId == userId))
                .GroupBy(message => message.DialogId)
                .Select(group => new MessageAlertsModel
                                {
                                    Message = group.OrderByDescending(message => message.SendOn).FirstOrDefault(),
                                    UnreadMessagesCount =
                                        group.Where(message => message.IsRead == false
                                                                    && message.ReceiverId == userId)
                                             .Count()
                                })
                .OrderByDescending(message => message.Message.SendOn)
                .Skip((page - 1) * 5)
                .Take(5)
                .Project().To<MessageViewModel>().ToList();

            var model = new MessagesPageViewModel()
            {
                Page = page + 1,
                Messages = messages,
            };

            return this.PartialView("_LastMessagesList", model);
        }

        [HttpGet]
        public ActionResult ViewFullConversation(string userName, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var currentUser = this.User.Identity.Name;
            var userId = this.User.Identity.GetUserId();
            var receiver = this.Data.Users.All().FirstOrDefault(user => user.UserName == userName);

            if (currentUser == userName)
            {
                this.TempData["error"] = "No cheating allowed!";
                return this.PartialView("Error");
            }

            if (receiver == null)
            {
                this.TempData["error"] = "Invalid message receiver!";
                return this.PartialView("Error");
            }

            var messages = this.Data.Messages.All()
                .Where(message => message.DialogId.Value != null && ((message.SenderId == userId && message.ReceiverId == receiver.Id) || (message.SenderId == receiver.Id && message.ReceiverId == userId)))
                .OrderByDescending(message => message.SendOn)
                .Skip((page - 1) * 5)
                .Take(5);

            var conversationMessages = messages.Project().To<ConversationViewModel>().ToList();
            conversationMessages.Reverse();

            bool areAnyUnreadMessages = false;

            foreach (var message in messages)
            {
                if (message.ReceiverId == userId && message.IsRead == false)
                {
                    message.IsRead = true;
                    areAnyUnreadMessages = true;
                }
            }

            if (areAnyUnreadMessages)
            {
                this.Data.Messages.SaveChanges();
            }

            var model = new ConversationPageViewModel()
            {
                Page = page + 1,
                Messages = conversationMessages,                
                ParticipantName = receiver.UserName,
                ParticipantPictureUrl = receiver.ImageUrl,
            };

            return this.PartialView("_ViewFullConversation", model);
        }

        // [HttpGet]
        // public ActionResult ViewDialog(int dialogId, int page = 1)
        // {
        // var userId = this.User.Identity.GetUserId();
        // var dialogParticipant = this.Data.DialogParticipants.All()
        // .FirstOrDefault(participant => participant.DialogId == dialogId && participant.UserId == userId);        //    if (dialogParticipant == null)
        // {
        // this.TempData["error"] = "You are not part from this conversation.";
        // return this.PartialView("_Message");
        // }
        // var messages = this.Data.Messages.All()
        // .OrderByDescending(message => message.SendOn)
        // .Where(message => message.DialogId == dialogId && message.SendOn > dialogParticipant.DateAdded)
        // .Skip((page - 1) * 5)
        // .Take(5)
        // .Project().To<DialogMessagesViewModel>()
        // .ToList();
        // messages.Reverse();
        // DialogViewModel model = new DialogViewModel()
        // {
        // Page = page + 1,
        // DialogId = dialogId,
        // Messages = messages,
        // };

        // if (page == 1)
        // {
        // model.ParticipantsInfo = this.Data.DialogParticipants.All()
        // .Where(participant => participant.DialogId == dialogId)
        // .Select(participant =>
        // new MessageParticipantInfo()
        // {
        // ParticipantName = participant.User.UserName,
        // ParticipantPictureUrl = participant.User.ImageUrl,
        // }
        // );
        // }
        // return this.PartialView("_DialogViewModel", model);
        // }
    }
}