using System;
using System.Collections.Generic;
using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using AgnesBot.Modules.CommentModule.Domain;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;

namespace AgnesBot.Modules.CommentModule.Tests
{
    [TestFixture]
    public class CommentModuleFixture
    {
        private CommentModule _module;
        private IIrcClient _client;
        private ICommentRepository _repository;
        
        [SetUp]
        public void SetUp()
        {
            _client = MockRepository.GenerateStub<IIrcClient>();
            _repository = MockRepository.GenerateStub<ICommentRepository>();
            _module = new CommentModule(_client, _repository);

            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            var unitOfWorkFactory = MockRepository.GenerateStub<IUnitOfWorkFactory>();
            unitOfWorkFactory.Stub(x => x.Create()).Return(unitOfWork);
            
            var container = MockRepository.GenerateStub<IWindsorContainer>();
            container.Stub(x => x.Resolve<IUnitOfWorkFactory>())
                .Return(unitOfWorkFactory);
            
            IoC.Initialize(container);
        }

        [Test]
        public void Can_Add_Comment()
        {
            // Arrange
            const string COMMENT = "testing 123";

            var data = new IrcMessageData() { Message = "!comments add " + COMMENT, Type = ReceiveType.ChannelMessage };

            SystemTime.Now = () => new DateTime(2009, 1, 1);

            // Act
            _module.Process(data);

            // Assert
            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Channel, "Comment has been added."));
            _repository.AssertWasCalled(
                repository => repository.CreateComment(Arg<Comment>.Matches(x =>
                                                                          x.Text == COMMENT
                                                                          &&
                                                                          x.Timestamp == SystemTime.Now())));
        }

        [Test]
        public void Does_Not_Add_Empty_Comment()
        {
            // Arrange
            var data = new IrcMessageData() { Message = "!comments add" };

            // Act
            _module.Process(data);

            // Assert
            _repository.AssertWasNotCalled(repository => repository.CreateComment(null), opt => opt.IgnoreArguments());
            _client.AssertWasNotCalled(client => client.SendMessage(SendType.Notice, null, null), opt => opt.IgnoreArguments());
        }

        [Test]
        public void Can_Find_Comments()
        {
            // Arrange
            const string term = "abc";

            var comments = new List<Comment>
                               {
                                   new Comment {Text = "testing1"},
                                   new Comment {Text = "testing2"},
                                   new Comment {Text = "testing3"}
                               };

            var data = new IrcMessageData { Message = "!comments find " + term, Type = ReceiveType.ChannelMessage };

            _repository.Stub(repository => repository.SearchComments(term))
                .Return(comments);

            // Act
            _module.Process(data);

            // Assert
            foreach(var comment in comments)
                _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Channel, string.Format("{0} on {1}", comment.Text, comment.Timestamp)));
        }
    }
}
