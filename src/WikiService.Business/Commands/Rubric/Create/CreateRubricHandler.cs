﻿using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.WikiService.Data.Provider;
using LT.DigitalOffice.WikiService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Business.Commands.Rubric.Create
{
  public class CreateRubricHandler : IRequestHandler<CreateRubricRequest, Guid?>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProvider _provider;

    private async Task<Guid?> CreateAsync(DbRubric dbRubric, CancellationToken ct)
    {
      if (dbRubric is null)
      {
        return null;
      }

      await _provider.Rubrics.AddAsync(dbRubric, ct);
      await _provider.SaveAsync();

      return dbRubric.Id;
    }

    private DbRubric Map(CreateRubricRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbRubric
      {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        ParentId = request.ParentId,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId()
      };
    }

    public CreateRubricHandler(
      IHttpContextAccessor httpContextAccessor,
      IDataProvider provider)
    {
      _httpContextAccessor = httpContextAccessor;
      _provider = provider;
    }

    public async Task<Guid?> Handle(CreateRubricRequest request, CancellationToken ct)
    {
      return await CreateAsync(Map(request), ct);
    }
  }
}