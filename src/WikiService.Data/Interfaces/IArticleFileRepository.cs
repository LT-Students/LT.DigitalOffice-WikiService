using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.WikiService.Models.Db;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Data.Interfaces
{
    [AutoInject]
    public interface IArticleFileRepository
    {
        Task<List<Guid>> CreateAsync(List<DbArticleFile> files);
    }
}