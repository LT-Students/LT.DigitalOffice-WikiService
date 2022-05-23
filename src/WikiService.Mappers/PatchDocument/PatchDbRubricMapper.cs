using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.WikiService.Mappers.PatchDocument.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Rubric;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.WikiService.Mappers.PatchDocument
{
  public class PatchDbRubricMapper : IPatchDbRubricMapper
  {
    public JsonPatchDocument<DbRubric> Map(JsonPatchDocument<EditRubricRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      var dbRequest = new JsonPatchDocument<DbRubric>();

      foreach (var item in request.Operations)
      {
        dbRequest.Operations.Add(new Operation<DbRubric>(item.op, item.path, item.from, item.value));
      }

      return dbRequest;
    }
  }
}
