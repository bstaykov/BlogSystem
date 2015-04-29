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
        public ActionResult VotePost(int id, bool vote)
        {
            this.ActualPostVote(id, vote);

            return this.PartialView("_LikingFeedbackMessage");
        }

        [HttpPost]
        public ActionResult VoteComment(int id, bool vote)
        {
            this.ActualCommentVote(id, vote);

            return this.PartialView("_LikingFeedbackMessage");
        }

        [NonAction]
        private void ActualPostVote(int postId, bool isLiked)
        {
            int postLikeValue = isLiked ? 1 : -1;
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
                post.Likes = post.Likes + postLikeValue;
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
                post.Likes = post.Likes + inverseLikeValue;
                this.Data.SaveChanges();
                this.TempData["success"] = "Vote inversed successfully.";
            }

            return;
        }

        [NonAction]
        private void ActualCommentVote(int commentId, bool isLiked)
        {
            int commentLikeValue = isLiked ? 1 : -1;
            int inverseLikeValue = isLiked ? 2 : -2;

            var userId = this.User.Identity.GetUserId();

            var comment = this.Data.Comments.GetById(commentId);

            if (comment == null)
            {
                this.TempData["error"] = "Comment is missing!";

                return;
            }
            else if (comment.UserId == userId)
            {
                this.TempData["error"] = "Cannot vote your comment!";

                return;
            }

            var commentLiker = this.Data.CommentLikers.All()
                .AsQueryable()
                .FirstOrDefault(cl => cl.CommentId == commentId && cl.UserId == userId);

            if (commentLiker == null)
            {
                var newCommentLiker = new CommentLiker()
                {
                    CommentId = commentId,
                    UserId = this.User.Identity.GetUserId(),
                    IsLiked = isLiked,
                };

                this.Data.CommentLikers.Add(newCommentLiker);
                comment.Likes = comment.Likes + commentLikeValue;
                this.Data.SaveChanges();

                this.TempData["success"] = "Comment voted successfully.";
            }
            else if (commentLiker.IsLiked == isLiked)
            {
                this.TempData["error"] = "Comment already voted by you!";
            }
            else
            {
                commentLiker.IsLiked = isLiked;
                comment.Likes = comment.Likes + inverseLikeValue;
                this.Data.SaveChanges();
                this.TempData["success"] = "Vote inversed successfully.";
            }

            return;
        }
    }
}