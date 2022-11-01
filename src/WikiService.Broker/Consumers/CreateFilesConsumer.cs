using DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Broker.Consumers
{
  public class CreateFilesConsumer : IConsumer<ICreateWikiFilesPublish>
  {
    private readonly IDataProvider _provider;

    private DbArticleFile Map(Guid fileId, Guid articleId)
    {
      return new DbArticleFile
      {
        Id = Guid.NewGuid(),
        FileId = fileId,
        ArticleId = articleId
      };
    }

    public CreateFilesConsumer(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task Consume(ConsumeContext<ICreateWikiFilesPublish> context)
    {
      if (context.Message.FilesIds is not null && context.Message.FilesIds.Any())
      {

/*        List<DbArticleFile> articleFiles = context.Message.FilesIds.Select(file => Map(file, context.Message.ArticleId)).ToList();

        await _provider.ArticlesFiles.AddRangeAsync(articleFiles);
        await _provider.SaveAsync();*/
      }
    }
  }
}
