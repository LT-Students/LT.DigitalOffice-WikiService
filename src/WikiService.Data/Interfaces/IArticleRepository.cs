using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IArticleRepository
  {
    Task<Guid?> CreateAsync(DbArticle dbArticle);
    Task<bool> DoesSameNameExistAsync(Guid rubricId, string articleName);
    Task<DbArticle> GetAsync(Guid articleId);
  }
}