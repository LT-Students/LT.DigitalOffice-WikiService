using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Models.Dto.Requests
{
  public record Rubric
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
    public bool HasChild { get; set; }
  }
}
