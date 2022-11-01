using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.WikiService.Broker.Requests.Interfaces;
using LT.DigitalOffice.WikiService.Business.Commands.Wiki;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.File.Find
{
  public class FindFilesHandler : IRequestHandler<FileFindFilter, FindResult<FileInfo>>
  {
    private readonly IDataProvider _provider;
    private readonly IFileService _fileService;

    private async Task<(List<DbArticleFile>, int filesCount)> FindAsync(FileFindFilter filter)
    {
      /*      if (filter is null)
            {
              return (null, 0);
            }

            IQueryable<DbProjectFile> dbFilesQuery = _provider.ProjectsFiles
              .AsQueryable();

            return (
              await dbFilesQuery.Where(file => file.ProjectId == filter.ProjectId && file.Access >= (int)access)
                .Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
              await dbFilesQuery.CountAsync());*/

      return (null, 0);
    }

    private FileInfo Map(FileCharacteristicsData file)
    {
      if (file is null)
      {
        return null;
      }

      return new FileInfo
      {
        Id = file.Id,
        Name = file.Name,
        Extension = file.Extension,
        Size = file.Size,
        CreatedAtUtc = file.CreatedAtUtc
      };
    }

    public FindFilesHandler(
      IDataProvider provider,
      IFileService fileService)
    {
      _provider = provider;
      _fileService = fileService;
    }

    public async Task<FindResult<FileInfo>> Handle(FileFindFilter request, CancellationToken ct)
    {
      (List<DbArticleFile> dbFiles, int totalCount) = await FindAsync(request);

      List<FileCharacteristicsData> files = await _fileService.GetFilesCharacteristicsAsync(dbFiles?.Select(file => file.FileId).ToList(), null);

      return new FindResult<FileInfo>
      {
        Body = files?.Select(Map).ToList(),
        TotalCount = totalCount
      };
    }
  }
}
