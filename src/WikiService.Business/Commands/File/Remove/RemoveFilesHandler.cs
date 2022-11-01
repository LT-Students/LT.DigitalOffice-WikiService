using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.ProjectService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.WikiService.Broker.Requests.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.File.Find;
using LT.DigitalOffice.WikiService.Data.Provider;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.File.Remove
{
  public class RemoveFilesHandler : IRequestHandler<RemoveFilesRequest, bool>
  {
    private readonly IDataProvider _provider;
    private readonly IPublish _publish;

    private async Task<bool> RemoveAsync(List<Guid> filesIds)
    {
/*      if (filesIds is null)
      {
        return false;
      }

      _provider.ArticlesFiles.RemoveRange(
        _provider.ArticlesFiles
        .Where(x => filesIds.Contains(x.FileId)));

      await _provider.SaveAsync();

      return true;*/

      return false;
    }

    public RemoveFilesHandler(
      IDataProvider provider,
      IPublish publish)
    {
      _provider = provider;
      _publish = publish;
    }

    public async Task<bool> Handle(RemoveFilesRequest request, CancellationToken cancellationToken)
    {
      bool response = await RemoveAsync(request.FilesIds);

      if (!response)
      {
        throw new BadRequestException();
      }

      await _publish.RemoveFilesAsync(request.FilesIds);

      return response;
    }
  }
}
