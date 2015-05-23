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
        //[HttpGet]
        //public ActionResult SendGlobalMessage(string dialogId)
        //{
        //    var model = new GlobalMessageInputModel()
        //    {
        //        DialogId = dialogId,
        //    };

        //    return this.PartialView("_SendGlobalMessage", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SendGlobalMessage(GlobalMessageInputModel model)
        //{
        //    var userName = this.User.Identity.Name;
        //    var userId = this.User.Identity.GetUserId();

        //    var dialog = this.Data.Dialogs.GetById(model.DialogId);

        //    if (dialog == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid dialog!");
        //    }

        //    var isUserParticipant = dialog.DialogParticipants.FirstOrDefault(participant => participant.UserId == userId);

        //    if (isUserParticipant == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Private conversation!");
        //    }

        //    if (ModelState.IsValid == false)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid model state!");
        //        return this.PartialView("_SendGlobalMessage", model);
        //    }

        //    // Find personal dialog (Dialog can have more than 2 participants)
        //    var dialog = this.Data.Dialogs.All()
        //        .Where(message =>
        //            message.DialogParticipants.Count() == 2
        //                && (message.DialogParticipants.FirstOrDefault(participant => participant.User.UserName == userName) != null
        //                        && message.DialogParticipants.FirstOrDefault(participant => participant.User.UserName == model.UserName) != null))
        //        .FirstOrDefault();

        //    if (dialog == null)
        //    {
        //        dialog = new Dialog()
        //        {
        //            StartedOn = DateTime.Now,
        //            StarterId = userId,
        //        };

        //        this.Data.Dialogs.Add(dialog);
        //        this.Data.SaveChanges();
        //        var messageId = dialog.Id;

        //        var senderParticipant = new DialogParticipant()
        //        {
        //            UserId = userId,
        //            DialogId = messageId,
        //            DateAdded = DateTime.Now,
        //        };

        //        var receiverParticipant = new DialogParticipant()
        //        {
        //            UserId = receiver.Id,
        //            DialogId = messageId,
        //            DateAdded = DateTime.Now,
        //        };

        //        this.Data.DialogParticipants.Add(senderParticipant);
        //        this.Data.DialogParticipants.Add(receiverParticipant);
        //        this.Data.SaveChanges();
        //    }

        //    var dateSent = DateTime.Now;
        //    var messageContent = new Message()
        //    {
        //        DialogId = dialog.Id,
        //        UserId = userId,
        //        SendOn = dateSent,
        //        Content = model.Content,
        //    };

        //    this.Data.Messages.Add(messageContent);
        //    //var notInformedParticipantsIds = this.Data.ReadDialogs.All()
        //    //    .Where(d => d.IsRead == true)
        //    //    .ToList();
        //    //foreach (var participant in notInformedParticipantsIds)
        //    //{
        //    //    participant.IsRead = false;
        //    //}
        //    this.Data.SaveChanges();

        //    return this.PartialView("_SendPrivateMessageLink", model.UserName);
        //}

        //[HttpGet]
        //public ActionResult SendPrivateMessage(string userName)
        //{
        //    var model = new PrivateMessageInputModel()
        //    {
        //        UserName = userName,
        //    };

        //    return this.PartialView("_SendPrivateMessage", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SendPrivateMessage(PrivateMessageInputModel model)
        //{
        //    var userName = this.User.Identity.Name;
        //    var userId = this.User.Identity.GetUserId();

        //    if (userName == model.UserName)
        //    {
        //        ModelState.AddModelError(string.Empty, "Can not send message to yourself!");
        //    }

        //    var receiver = this.Data.Users.All().FirstOrDefault(user => user.UserName == model.UserName);

        //    if (receiver == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid message receiver!!!");
        //    }

        //    if (ModelState.IsValid == false)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid model state!");
        //        return this.PartialView("_SendPrivateMessage", model);
        //    }

        //    // Find personal dialog (Dialog can have more than 2 participants)
        //    var dialog = this.Data.Dialogs.All()
        //        .Where(message =>
        //            message.DialogParticipants.Count() == 2
        //                && (message.DialogParticipants.FirstOrDefault(participant => participant.User.UserName == userName) != null
        //                        && message.DialogParticipants.FirstOrDefault(participant => participant.User.UserName == model.UserName) != null))
        //        .FirstOrDefault();

        //    if (dialog == null)
        //    {
        //        dialog = new Dialog()
        //        {
        //            StartedOn = DateTime.Now,
        //            StarterId = userId,
        //        };

        //        this.Data.Dialogs.Add(dialog);
        //        this.Data.SaveChanges();

        //        var senderParticipant = new DialogParticipant()
        //        {
        //            UserId = userId,
        //            DialogId = dialog.Id,
        //            DateAdded = DateTime.Now,
        //        };

        //        var receiverParticipant = new DialogParticipant()
        //        {
        //            UserId = receiver.Id,
        //            DialogId = dialog.Id,
        //            DateAdded = DateTime.Now,
        //        };

        //        this.Data.DialogParticipants.Add(senderParticipant);
        //        this.Data.DialogParticipants.Add(receiverParticipant);
        //        this.Data.ReadDialogs.Add(new ReadDialog() 
        //            {
        //                DialogId = dialog.Id,
        //                UserId = userId,
        //                IsRead = true,
        //            }
        //        );
        //        this.Data.ReadDialogs.Add(new ReadDialog()
        //        {
        //            DialogId = dialog.Id,
        //            UserId = receiver.Id,
        //            IsRead = false,
        //        }
        //        );
        //        this.Data.SaveChanges();
        //    }

        //    var messageContent = new Message()
        //    {
        //        DialogId = dialog.Id,
        //        UserId = userId,
        //        SendOn = DateTime.Now,
        //        Content = model.Content,
        //    };

        //    this.Data.Messages.Add(messageContent);
        //    this.Data.SaveChanges();

        //    return this.PartialView("_SendPrivateMessageLink", model.UserName);
        //}

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
        public ActionResult SendPrivateMessage(MessageInputModel model)
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

            return this.PartialView("_SendMessageLink", model.UserName);
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
                    .Take(1)
                    .Project().To<MessageViewModel>()
                    .FirstOrDefault();

                messageViewModel.ParticipantsInfo = this.Data.DialogParticipants.All()
                    .Where(participant => participant.DialogId == id && participant.UserId != userId)
                    .Select(participant => 
                        new MessageParticipantInfo()
                        {
                            ParticipantName = participant.User.UserName,
                            ParticipantPictureUrl = participant.User.ImageUrl,
                        }
                    );

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

        [HttpGet]
        public ActionResult ViewDialog(int dialogId, int page = 1)
        {
            var userId = this.User.Identity.GetUserId();

            var dialogParticipant = this.Data.DialogParticipants.All()
                .FirstOrDefault(participant => participant.DialogId == dialogId && participant.UserId == userId);

            if (dialogParticipant == null)
            {
                this.TempData["error"] = "You are not part from this conversation.";
                return this.PartialView("_Message");
            }

            var messages = this.Data.Messages.All()
                .OrderByDescending(message => message.SendOn)
                .Where(message => message.DialogId == dialogId && message.SendOn > dialogParticipant.DateAdded)
                .Skip((page - 1) * 5)
                .Take(5)
                .Project().To<DialogMessagesViewModel>()
                .ToList();

            messages.Reverse();

            DialogViewModel model = new DialogViewModel()
            {
                Page = page + 1,
                DialogId = dialogId,
                Messages = messages,
            };

            if (page == 1)
            {
                model.ParticipantsInfo = this.Data.DialogParticipants.All()
                .Where(participant => participant.DialogId == dialogId)
                .Select(participant =>
                    new MessageParticipantInfo()
                    {
                        ParticipantName = participant.User.UserName,
                        ParticipantPictureUrl = participant.User.ImageUrl,
                    }
                );
            }

            return this.PartialView("_DialogViewModel", model);
        }
    }
}