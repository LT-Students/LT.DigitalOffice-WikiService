using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record SubRubricInfo
  {
    public Guid SubRubricId { get; set; }
    public string SubRubricName { get; set; }
  }
}
