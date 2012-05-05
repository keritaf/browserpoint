namespace Itransition.BrowserPoint.Model.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// EF migrations configuration
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<Itransition.BrowserPoint.Model.ModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        ///  This method will be called after migrating to the latest version.
        /// 
        ///  You can use the <code>DbSet<T>.AddOrUpdate()</code> helper extension method 
        ///  to avoid creating duplicate seed data. E.g.
        ///
        ///  <code>context.People.AddOrUpdate(
        ///      p => p.FullName,
        ///      new Person { FullName = "Andrew Peters" },
        ///      new Person { FullName = "Brice Lambson" },
        ///      new Person { FullName = "Rowan Miller" }
        ///    );</code>
        ///
        ///
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(Itransition.BrowserPoint.Model.ModelContext context)
        {
        }
    }
}
