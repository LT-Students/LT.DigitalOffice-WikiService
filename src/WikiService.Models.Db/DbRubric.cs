using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.WikiService.Models.Db
{
  public class DbRubric
  {
    public const string TableName = "Rubrics";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public int? ChildCount { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public ICollection<DbArticle> Articles { get; set; }

    public DbRubric()
    {
      Articles = new HashSet<DbArticle>();
    }
  }

  public class DbRubricConfiguration : IEntityTypeConfiguration<DbRubric>
  {
    public void Configure(EntityTypeBuilder<DbRubric> builder)
    {
      builder
        .ToTable(DbRubric.TableName);

      builder
        .HasKey(r => r.Id);

      builder
        .HasMany(a => a.Articles)
        .WithOne(r => r.Rubric);
    }
  }
}
