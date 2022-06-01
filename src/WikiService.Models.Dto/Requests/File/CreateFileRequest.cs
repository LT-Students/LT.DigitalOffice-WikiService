using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Dto.Models
{
    public record CreateFileRequest
    {
        public Guid ArticleId { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}