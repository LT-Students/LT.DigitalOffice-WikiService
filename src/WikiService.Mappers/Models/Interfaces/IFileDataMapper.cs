using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.File;
using LT.DigitalOffice.WikiService.Models.Dto.Models;

namespace LT.DigitalOffice.WikiService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IFileDataMapper
  {
    FileData Map(FileInfo file);
  }
}
