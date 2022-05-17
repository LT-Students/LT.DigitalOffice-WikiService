using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests
{
  public record CreateRubricRequest
  {
    [Required]
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
  }
}
