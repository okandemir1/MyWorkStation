namespace OkanDemir.Data
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OkanDemir.Model;
    using OkanDemir.Model.Base;
    public class OkanDemirDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceType> InvoiceTypes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<IncomeType> IncomeTypes { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<ArchiveCategory> ArchiveCategories { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteHashtag> NoteHashtags { get; set; }
        public DbSet<NoteMetion> NoteMetions { get; set; }
        public DbSet<Metion> Metions { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<CodeNote> CodeNotes { get; set; }
        public DbSet<CodeCategory> CodeCategories { get; set; }
        public DbSet<TodoImage> TodoImages { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoListType> TodoListTypes { get; set; }
        public DbSet<TodoProject> TodoProjects { get; set; }
        public DbSet<TodoTable> TodoTables { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePage> RolePages { get; set; }

        public OkanDemirDbContext(DbContextOptions<OkanDemirDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            CheckDeletable();
            AddTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            CheckDeletable();
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            CheckDeletable();
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            CheckDeletable();
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void CheckDeletable()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntityWithDeletable && (x.State == EntityState.Deleted && ((BaseEntityWithDeletable)x.Entity).IsDeletable != true));

            if (entities != null && entities.Any())
            {
                throw new Exception("Seçilen veri silinemez");
            }
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntityWithDate && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntityWithDate)entity.Entity).CreateDate = DateTime.Now;
                }

                ((BaseEntityWithDate)entity.Entity).UpdateDate = DateTime.Now;
            }
        }

    }
}