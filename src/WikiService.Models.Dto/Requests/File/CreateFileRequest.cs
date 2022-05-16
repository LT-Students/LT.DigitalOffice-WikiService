using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
  public record CreateFileRequest
  {
    [Required]
    public string Name { get; set; }
    public string Content { get; set; }
    [Required]
    public string Extension { get; set; }
  }
}