using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IArticleRepository
  {
    Task<Guid?> CreateAsync(DbArticle dbArticle);
    Task<bool> DoesSameNameExistAsync(Guid rubricId, string articleName);
    Task<DbArticle> GetAsync(Guid articleId);
    Task<bool> EditAsync(Guid articleId, JsonPatchDocument<DbArticle> request);
  }
}