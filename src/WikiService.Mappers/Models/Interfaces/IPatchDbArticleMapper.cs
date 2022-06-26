using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.WikiService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IPatchDbArticleMapper
  {
    JsonPatchDocument<DbArticle> Map(JsonPatchDocument<EditArticleRequest> request);
  }
}