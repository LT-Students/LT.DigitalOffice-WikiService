using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.WikiService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    DbSet<DbRubric> Rubrics { get; set; }
    DbSet<DbArticle> Articles { get; set; }
    DbSet<DbArticleFile> ArticlesFiles { get; set; }
  }
}
