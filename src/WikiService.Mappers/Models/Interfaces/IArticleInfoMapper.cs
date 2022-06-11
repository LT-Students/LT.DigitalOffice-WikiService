﻿using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;

namespace LT.DigitalOffice.WikiService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IArticleInfoMapper
  {
    ArticleInfo Map(DbArticle dbArticle);
  }
}