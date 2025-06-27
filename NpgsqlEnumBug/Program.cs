using Microsoft.EntityFrameworkCore;

using var db = new AppContext();

var list = db.Persons.Where(x => x.CurrentMood == Mood.Happy).ToList();

Console.WriteLine($"Found {list.Count} persons with mood Happy");

internal class PersonEntity
{
    public string? Name { get; set; }

    public Mood CurrentMood { get; set; }
}

internal enum Mood
{
    Sad,
    Ok,
    Happy
}

internal class AppContext : DbContext
{
    public virtual DbSet<PersonEntity> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
            "Username=test;Password=test;Host=localhost;Port=5432;Database=d_app;",
            builder => { builder.MapEnum<Mood>("mood"); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("app");

        modelBuilder.Entity<PersonEntity>(
            entity =>
            {
                entity.HasKey(e => e.Name);
                entity.ToTable("person");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.CurrentMood).HasColumnName("current_mood");
            });
    }
}
