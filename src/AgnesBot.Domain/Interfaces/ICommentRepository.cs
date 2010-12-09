namespace AgnesBot.Domain.Interfaces
{
    public interface ICommentRepository
    {
        void SaveComment(Comment comment);
    }
}