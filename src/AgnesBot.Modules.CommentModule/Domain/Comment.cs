﻿using System;

namespace AgnesBot.Modules.CommentModule.Domain
{
    public class Comment
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
