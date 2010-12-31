using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.Repositories;
using AgnesBot.Modules.QuoteModule.Domain;
using AgnesBot.Modules.QuoteModule.Indexes;

namespace AgnesBot.Modules.QuoteModule.Repositories
{
    public class QuoteRepository : BaseRepository, IQuoteRepository
    {
        public void CreateQuote(Quote quote)
        {
            Session.Store(quote);
        }

        public IList<Quote> SearchQuotes(string text, int limit)
        {
            return Session.Query<Quote, Quotes_ByText>()
                .Where(quote => quote.Text.Contains(text))
                .Take(limit)
                .ToList();
        }
    }
}
