using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Requests.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbArticleMapper
  {
    DbArticle Map(CreateArticleRequest request);
  }
}
