using System.Collections.Generic;
using AgnesBot.Modules.QuoteModule.Domain;

namespace AgnesBot.Modules.QuoteModule.Repositories
{
    public interface IQuoteRepository
    {
        void CreateQuote(Quote quote);
        IList<Quote> SearchQuotes(string text, int limit);
    }
}