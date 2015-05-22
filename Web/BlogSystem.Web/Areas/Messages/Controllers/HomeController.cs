namespace BlogSystem.Web.Areas.Messages.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            var dialog = this.Data.Dialogs.All()
                .Where(message =>
                    message.DialogParticipants.Count() == 2
                        && (message.DialogParticipants.FirstOrDefault(participant => participant.User.UserName == userName) != null
                                && message.DialogParticipants.FirstOrDefault(participant => participant.User.UserName == model.UserName) != null))
                .FirstOrDefault();

            // var dialogMessage = this.Data.Messages.All()
            // .Where(message =>
            // message.MessageParticipants
            // .Where(participant => participant.MessageId == message.Id
            // && (participant.User.UserName == userName
            // || participant.User.UserName == model.UserName))
            // .Count() == 2)
            // //.Select(message => message.Id)
            // .FirstOrDefault();
            if (dialog == null)
            {
                dialog = new Dialog()
                {
                    StartedOn = DateTime.Now,
                    StarterId = userId,
                };

                this.Data.Dialogs.Add(dialog);
                this.Data.SaveChanges();
                var messageId = dialog.Id;

                var senderParticipant = new DialogParticipant()
                {
                    UserId = userId,
                    DialogId = messageId,
                    DateAdded = DateTime.Now,
                };

                var receiverParticipant = new DialogParticipant()
                {
                    UserId = receiver.Id,
                    DialogId = messageId,
                    DateAdded = DateTime.Now,
                };

                this.Data.DialogParticipants.Add(senderParticipant);
                this.Data.DialogParticipants.Add(receiverParticipant);
                this.Data.SaveChanges();
            }

            var messageContent = new Message()
            {
                DialogId = dialog.Id,
                UserId = userId,
                SendOn = DateTime.Now,
                Content = model.Content,
            };

            this.Data.Messages.Add(messageContent);
            this.Data.SaveChanges();

            this.TempData["success"] = "Message sent.";

            return this.PartialView("_Message");
        }

        [HttpGet]
        public ActionResult GetMessages(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var userName = this.User.Identity.Name;
            var userId = this.User.Identity.GetUserId();

            // Messages participating in. Take last contents of these messages.
            var messagesParticipatingInIds = this.Data.DialogParticipants.All()
                .Where(participant => participant.UserId == userId)
                .OrderByDescending(participant => participant.DateAdded)
                .Select(participant => participant.DialogId)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToList();

            ICollection<MessageViewModel> messageViewModels = new List<MessageViewModel>();

            foreach (var id in messagesParticipatingInIds)
            {
                // id, sender, picUrl, date, content
                MessageViewModel messageViewModel = this.Data.Messages.All()
                    .OrderByDescending(message => message.SendOn)
                    .Where(message => message.DialogId == id)
                    .Project().To<MessageViewModel>().FirstOrDefault();

                // pictures
                messageViewModel.ParticipantsPicturesUrls = this.Data.DialogParticipants.All()
                    .Where(message => message.DialogId == id)
                    .Select(message => message.User.ImageUrl);

                // participants
                messageViewModel.ParticipantsNames = this.Data.DialogParticipants.All()
                    .Where(message => message.DialogId == id)
                    .Select(message => message.User.UserName);

                messageViewModels.Add(messageViewModel);
            }

            // var messageIds = this.Data.MessagesContents.All()
            // .Where(message => message.User.UserName == userName)
            // .OrderByDescending(message => message.SendOn)
            // .Select(message => message.MessageId)
            // .Distinct()
            // .Skip((page - 1) * 5)
            // .Take(5)
            // .ToList();

            // var messages = this.Data.Messages.All()
            // .Project().To<MessageViewModel>()
            // .OrderByDescending(message => message.SendOn)
            // .Skip((page - 1) * 5)
            // .Take(5)
            // .ToList();
            var model = new MessagesPageViewModel()
            {
                Page = page + 1,
                Messages = messageViewModels,
            };

            return this.PartialView("_LastMessagesList", model);
        }
    }
}