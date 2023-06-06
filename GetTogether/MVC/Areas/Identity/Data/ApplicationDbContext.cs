using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.Areas.Identity.Data;
using MVC.Models;
using System.Reflection.Emit;

namespace MVC.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        builder.Entity<Group>()
            .HasOne(g => g.Owner)
            .WithMany(m => m.IsOwnerOfGroups)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Group>()
            .HasIndex(g => g.Name)
            .IsUnique();
        builder.Entity<ApplicationUserGroup>()
            .HasKey(ag => new { ag.GroupId, ag.MemberId });
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(30);
    }
}