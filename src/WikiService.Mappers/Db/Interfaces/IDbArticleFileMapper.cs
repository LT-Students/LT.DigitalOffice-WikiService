using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using System;

namespace LT.DigitalOffice.WikiService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbArticleFileMapper
  {
    DbArticleFile Map(Guid fileId, Guid articletId);
  }
}
