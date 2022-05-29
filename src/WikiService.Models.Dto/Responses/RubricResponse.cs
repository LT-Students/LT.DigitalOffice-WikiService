using LT.DigitalOffice.WikiService.Models.Db;
using LT.DigitalOffice.WikiService.Models.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Models.Dto.Responses
{
  public class RubricResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
    public IEnumerable<SubRubricInfo> SubRubrics { get; set; }
    public IEnumerable<ArticleInfo> Articles { get; set; }



  }
}
