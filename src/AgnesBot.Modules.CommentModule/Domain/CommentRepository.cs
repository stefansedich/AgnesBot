using System;
using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.Data;
using AgnesBot.Modules.CommentModule.Indexes;

namespace AgnesBot.Modules.CommentModule.Domain
{
    public interface ICommentRepository
    {
        void CreateComment(Comment comment);
        IList<Comment> SearchComments(string text);
    }

    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public void CreateComment(Comment comment)
        {
            Session.Store(comment);
        }

        public IList<Comment> SearchComments(string text)
        {
            return Session.Query<Comment, Comments_ByText>()
                .Where(comment => comment.Text.Contains(text))
                .Take(3)
                .ToList();
        }
    }
}
