namespace Itransition.BrowserPoint.Model
{
    using System.Data.Entity;
    using Itransition.BrowserPoint.Model.Entities;

    public class ModelContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
