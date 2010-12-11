﻿using AgnesBot.Core.Data;

namespace AgnesBot.Modules.CommentModule.Domain
{
    public interface ICommentRepository
    {
        void CreateComment(Comment comment);
    }

    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public void CreateComment(Comment comment)
        {
            Session.Store(comment);
        }
    }
}