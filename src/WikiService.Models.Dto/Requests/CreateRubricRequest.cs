using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests
{
  public record CreateRubricRequest
  {
    public string Name { get; set; }
    [Required]
    public Guid? ParentId { get; set; }
  }
}
