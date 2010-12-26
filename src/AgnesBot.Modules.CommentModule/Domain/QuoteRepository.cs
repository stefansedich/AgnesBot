using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.Data;
using AgnesBot.Modules.QuoteModule.Indexes;

namespace AgnesBot.Modules.QuoteModule.Domain
{
    public interface IQuoteRepository
    {
        void CreateQuote(Quote quote);
        IList<Quote> SearchQuotes(string text, int limit);
    }

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
