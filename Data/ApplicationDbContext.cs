using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCAuth1.Models;

namespace MVCAuth1.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<ImageStore> ImageStores {set; get;}
    public DbSet<Message> Messages{set; get;}
    public DbSet<Friend> FriendList{set; get;}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach(var entityType in builder.Model.GetEntityTypes())
        {
            string? tableName = entityType.GetTableName();
            if(tableName == null)
            {
                continue;
            }
            if(tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName[6..]);
            }
        }
    }
}