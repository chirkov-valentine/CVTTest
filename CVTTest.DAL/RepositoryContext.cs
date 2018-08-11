using System.Data.Entity;
using CVTTest.Domain.Calendar;
using CVTTest.Domain.ContactList;

namespace CVTTest.DAL
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() : base("name=RepositoryContext")
        {
            Database.SetInitializer<RepositoryContext>(new RepositoryContextInitializer());
        }
        
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}