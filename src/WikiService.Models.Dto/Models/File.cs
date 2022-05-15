using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record File
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
  }
}
