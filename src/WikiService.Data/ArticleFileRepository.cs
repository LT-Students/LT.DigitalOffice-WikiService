using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
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

        public async Task<List<Guid>> CreateAsync(List<DbArticleFile> files)
        {
            if (files == null)
            {
                return null;
            }

            _provider.ArticlesFiles.AddRange(files);
            await _provider.SaveAsync();

            return files.Select(x => x.FileId).ToList();
        }
    }
}