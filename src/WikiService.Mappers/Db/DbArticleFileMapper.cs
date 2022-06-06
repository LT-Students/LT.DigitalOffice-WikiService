using LT.DigitalOffice.WikiService.Mappers.Db.Interfaces;
using LT.DigitalOffice.WikiService.Models.Db;
using System;

namespace LT.DigitalOffice.WikiService.Mappers.Db
{
  public class DbArticleFileMapper : IDbArticleFileMapper
  {
    public DbArticleFile Map(Guid fileId, Guid articleId)
    {
      return new DbArticleFile
      {
        Id = Guid.NewGuid(),
        ArticleId = articleId,
        FileId = fileId
      };
    }
  }
}
