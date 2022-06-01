using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IArticleRepository
  {
    Task<Guid?> CreateAsync(DbArticle dbArticle);
    Task<bool> DoesSameNameExistAsync(Guid rubricId, string articleName);
    Task<bool> DoesExistAsync(Guid articleId);
  }
}