using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests.File
{
  public record RemoveFilesRequest
  {
    public List<Guid> FilesIds { get; set; }
  }
}

