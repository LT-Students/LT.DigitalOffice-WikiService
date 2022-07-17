using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
  [AutoInject]
  public interface IArticleFileRepository
  {
    public Task<bool> RemoveAsync(List<Guid> filesIds);
  }
}