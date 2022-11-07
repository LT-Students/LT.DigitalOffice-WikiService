using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.WikiService.Data.Provider;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.ProjectService.Broker
{
  public class CheckArticlesExistenceConsumer : IConsumer<ICheckArticlesExistence>
  {
    private readonly IDataProvider _provider;

    public CheckArticlesExistenceConsumer(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task Consume(ConsumeContext<ICheckArticlesExistence> context)
    {
      List<Guid> existArticles = await _provider.Articles
        .Where(article => context.Message.ArticlesIds.Contains(article.Id))
        .Select(article => article.Id).ToListAsync();

      object response = OperationResultWrapper.CreateResponse((_) => ICheckArticlesExistence.CreateObj(existArticles), context);

      await context.RespondAsync<IOperationResult<ICheckArticlesExistence>>(response);
    }
  }
}
