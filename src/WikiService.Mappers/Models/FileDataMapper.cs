using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.WikiService.Mappers.Models.Interfaces;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.WikiService.Mappers.Models
{
  public class FileDataMapper : IFileDataMapper
  {
    public FileData Map(FileInfo file)
    {
      if (file is null)
      {
        return null;
      }

      Guid fileId = Guid.NewGuid();

      return new FileData(
        fileId,
        file.Name,
        file.Content,
        file.Extension);
    }
  }
}
