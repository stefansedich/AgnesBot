namespace AgnesBot.Modules.CommentModule
{
    public interface ICommentRepository
    {
        void CreateComment(Comment comment);
    }

    public class CommentRepository : ICommentRepository
    {
        public CommentRepository() { }

        public void CreateComment(Comment comment)
        {
            
        }
    }
}
