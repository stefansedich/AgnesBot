using System.Linq;
using AgnesBot.Modules.QuoteModule.Domain;
using Raven.Client.Indexes;
using Raven.Database.Indexing;

namespace AgnesBot.Modules.QuoteModule.Indexes
{
    public class Quotes_ByText : AbstractIndexCreationTask
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition<Quote, Quote>
                       {
                           Map = quotes => from quote in quotes
                                             select new {quote.Text},
                           Indexes =
                               {
                                   {x => x.Text, FieldIndexing.Analyzed}
                               },
                           Stores =
                               {
                                   {x => x.Text, FieldStorage.Yes}
                               }
                       }.ToIndexDefinition(DocumentStore.Conventions);
        }
    }
}