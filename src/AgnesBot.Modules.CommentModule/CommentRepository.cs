namespace AgnesBot.Modules.CommentModule
{
    public interface ICommentRepository
    {
        void SaveComment(Comment comment);
    }

    public class CommentRepository : ICommentRepository
    {
        public CommentRepository() { }

        public void SaveComment(Comment comment)
        {
            
        }
    }
}
