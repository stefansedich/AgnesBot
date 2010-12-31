using System;
using System.Collections.Generic;
using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using AgnesBot.Modules.QuoteModule.Domain;
using AgnesBot.Modules.QuoteModule.Repositories;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;

namespace AgnesBot.Modules.QuoteModule.Tests
{
    [TestFixture]
    public class QuoteModuleFixture
    {
        private QuoteModule _module;
        private IIrcClient _client;
        private IQuoteRepository _repository;
        
        [SetUp]
        public void SetUp()
        {
            _client = MockRepository.GenerateStub<IIrcClient>();
            _repository = MockRepository.GenerateStub<IQuoteRepository>();
            _module = new QuoteModule(_client, _repository);

            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            var unitOfWorkFactory = MockRepository.GenerateStub<IUnitOfWorkFactory>();
            unitOfWorkFactory.Stub(x => x.Create()).Return(unitOfWork);
            
            var container = MockRepository.GenerateStub<IWindsorContainer>();
            container.Stub(x => x.Resolve<IUnitOfWorkFactory>())
                .Return(unitOfWorkFactory);
            
            IoC.Initialize(container);
        }

        [Test]
        public void Can_Add_Quote()
        {
            // Arrange
            const string QUOTE = "testing 123";

            var data = new IrcMessageData() { Message = "!quotes add " + QUOTE, Type = ReceiveType.ChannelMessage };

            SystemTime.Now = () => new DateTime(2009, 1, 1);

            // Act
            _module.Process(data);

            // Assert
            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Channel, "Quote has been added."));
            _repository.AssertWasCalled(
                repository => repository.CreateQuote(Arg<Quote>.Matches(x =>
                                                                          x.Text == QUOTE
                                                                          &&
                                                                          x.Timestamp == SystemTime.Now())));
        }

        [Test]
        public void Does_Not_Add_Empty_Quote()
        {
            // Arrange
            var data = new IrcMessageData() { Message = "!quotes add" };

            // Act
            _module.Process(data);

            // Assert
            _repository.AssertWasNotCalled(repository => repository.CreateQuote(null), opt => opt.IgnoreArguments());
            _client.AssertWasNotCalled(client => client.SendMessage(SendType.Notice, null, null), opt => opt.IgnoreArguments());
        }

        [Test]
        public void Can_Find_Quotes()
        {
            // Arrange
            const string term = "abc";

            var comments = new List<Quote>
                               {
                                   new Quote {Text = "testing1"},
                                   new Quote {Text = "testing2"},
                                   new Quote {Text = "testing3"}
                               };

            var data = new IrcMessageData { Message = "!quotes find " + term, Type = ReceiveType.ChannelMessage };

            _repository.Stub(repository => repository.SearchQuotes(term, 3))
                .Return(comments);

            // Act
            _module.Process(data);

            // Assert
            foreach(var comment in comments)
                _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Channel, string.Format("{0} on {1}", comment.Text, comment.Timestamp)));
        }
    }
}
