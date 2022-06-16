using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Provider.MsSql.Ef
{
  public class WikiServiceDbContext : DbContext, IDataProvider
  {
    public DbSet<DbRubric> Rubrics { get; set; }
    public DbSet<DbArticle> Articles { get; set; }
    public DbSet<DbArticleFile> ArticlesFiles { get; set; }

    public WikiServiceDbContext(DbContextOptions<WikiServiceDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.WikiService.Models.Db"));
    }

    public void EnsureDeleted()
    {
      Database.EnsureDeleted();
    }

    public bool IsInMemory()
    {
      return Database.IsInMemory();
    }

    public object MakeEntityDetached(object obj)
    {
      Entry(obj).State = EntityState.Detached;
      return Entry(obj).State;
    }

    void IBaseDataProvider.Save()
    {
      SaveChanges();
    }

    async Task IBaseDataProvider.SaveAsync()
    {
      await SaveChangesAsync();
    }
  }
}
