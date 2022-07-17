using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Broker.Publishes.Interfaces
{
  [AutoInject]
  public interface IPublish
  {
    Task RemoveFilesAsync(List<Guid> filesIds);
  }
}

