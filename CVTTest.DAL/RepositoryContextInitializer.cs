using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVTTest.Domain.Calendar;

namespace CVTTest.DAL
{
    public class RepositoryContextInitializer : DropCreateDatabaseIfModelChanges<RepositoryContext>
    {
        protected override void Seed(RepositoryContext context)
        {
            context.EventTypes.Add(new EventType {EventTypeName = "Встреча"});
            context.EventTypes.Add(new EventType { EventTypeName = "Дело" });
            context.EventTypes.Add(new EventType { EventTypeName = "Памятка" });
        }
    }
}
