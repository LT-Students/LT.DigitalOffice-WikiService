using DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.WikiService.Business.Commands.File.Find
{
  public record class FileFindFilter : BaseFindFilter, IRequest<FindResult<FileInfo>>
  {
    [FromQuery(Name = "particleid")]
    public Guid ArticleId { get; set; }
  }
}
