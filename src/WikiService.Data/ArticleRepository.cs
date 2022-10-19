using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data
{
  public class ArticleRepository : IArticleRepository
  {
    private readonly IDataProvider _provider;

    public ArticleRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> DoesSameNameExistAsync(Guid rubricId, string articleName)
    {
      return await _provider.Articles.AnyAsync(a => a.RubricId == rubricId && a.Name == articleName);
    }
  }
}