using AgnesBot.Domain;
using AgnesBot.Domain.Interfaces;
using Raven.Client.Document;

namespace AgnesBot.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(DocumentStore store) : base(store) { }

        public void SaveComment(Comment comment)
        {
            using(var session = Store.OpenSession())
            {
                session.Store(comment);
                session.SaveChanges();
            }
        }
    }
}
