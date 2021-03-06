using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.WikiService.Mappers.Models
{
  public class PatchDbArticleMapper : IPatchDbArticleMapper
  {
    public JsonPatchDocument<DbArticle> Map(JsonPatchDocument<EditArticleRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbArticle> result = new JsonPatchDocument<DbArticle>();

      foreach (Operation<EditArticleRequest> item in request.Operations)
      {
        result.Operations.Add(new Operation<DbArticle>(item.op, item.path, item.from, item.value));
      }

      return result;
    }
  }
}