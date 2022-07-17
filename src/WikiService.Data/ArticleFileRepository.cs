using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data
{
  public class ArticleFileRepository : IArticleFileRepository
  {
    private readonly IDataProvider _provider;

    public ArticleFileRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }
    public async Task<bool> RemoveAsync(List<Guid> filesIds)
    {
      if (await _provider
        .ArticlesFiles
        .AnyAsync(x => filesIds.Contains(x.FileId)))
      {
        _provider.ArticlesFiles.RemoveRange(
          _provider.ArticlesFiles
          .Where(x => filesIds.Contains(x.FileId)));

        await _provider.SaveAsync();

        return true;
      }
      return false;
    }
  }
}