using DigitalOffice.Kernel.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.WikiService.Business.Commands.File.Remove
{
  public record RemoveFilesRequest : IRequest<bool>
  {
    [Required]
    public Guid ArticleId { get; set; }
    [Required]
    public List<Guid> FilesIds { get; set; }
  }
}
