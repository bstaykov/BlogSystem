namespace BlogSystem.Web.Areas.Likes.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;

    using BlogSystem.Models;
    using BlogSystem.Web.Controllers;
    using Microsoft.AspNet.Identity;

    [Authorize]
    public class HomeController : BaseController
    {
        [HttpPost]
        public ActionResult UpVoteById(int id)
        {
            this.ActualVote(id, true);

            return this.PartialView("_LikingFeedbackMessage");
        }

        [HttpPost]
        public ActionResult DownVoteById(int id)
        {
            this.ActualVote(id, false);

            return this.PartialView("_LikingFeedbackMessage");
        }

        [NonAction]
        private void ActualVote(int postId, bool isLiked)
        {
            int postsLikeValue = isLiked ? 1 : -1;
            int inverseLikeValue = isLiked ? 2 : -2;

            var userId = this.User.Identity.GetUserId();

            var post = this.Data.Posts.GetById(postId);

            if (post == null)
            {
                this.TempData["error"] = "Post is missing!";

                return;
            }
            else if (post.UserId == userId)
            {
                this.TempData["error"] = "Cannot vote your post!";

                return;
            }

            var postLiker = this.Data.PostLikers.All()
                .AsQueryable()
                .FirstOrDefault(pl => pl.PostId == postId && pl.UserId == userId);

            if (postLiker == null)
            {
                var newPostLiker = new PostLiker()
                {
                    PostId = postId,
                    UserId = userId,
                    IsLiked = isLiked,
                };

                this.Data.PostLikers.Add(newPostLiker);
                post.Likes = post.Likes  + postsLikeValue;
                this.Data.SaveChanges();

                this.TempData["success"] = "Post voted successfully.";
            }
            else if (postLiker.IsLiked == isLiked)
            {
                this.TempData["error"] = "Post already voted by you!";
            }
            else
            {
                postLiker.IsLiked = isLiked;
                post.Likes = post.Likes  + inverseLikeValue;
                this.Data.SaveChanges();
                this.TempData["success"] = "Post voted successfully.";
            }

            return;
        }
    }
}