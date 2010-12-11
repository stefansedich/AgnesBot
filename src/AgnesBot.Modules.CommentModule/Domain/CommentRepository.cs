using System;
using System.Collections;
using System.Linq;
using AgnesBot.Core.Data;

namespace AgnesBot.Modules.CommentModule.Domain
{
    public interface ICommentRepository
    {
        void CreateComment(Comment comment);
        Comment SearchComments(string text);
    }

    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public void CreateComment(Comment comment)
        {
            Session.Store(comment);
        }

        public Comment SearchComments(string text)
        {
            return Session.Query<Comment>()
                .Where(comment => comment.Text.Contains(text))
                .FirstOrDefault();
        }
    }
}
