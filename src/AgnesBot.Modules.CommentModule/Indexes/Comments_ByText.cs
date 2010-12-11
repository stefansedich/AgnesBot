using System.Linq;
using AgnesBot.Modules.CommentModule.Domain;
using Raven.Client.Indexes;
using Raven.Database.Indexing;

namespace AgnesBot.Modules.CommentModule.Indexes
{
    public class Comments_ByText : AbstractIndexCreationTask
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition<Comment, Comment>
                       {
                           Map = comments => from comment in comments
                                             select new {comment.Text},
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