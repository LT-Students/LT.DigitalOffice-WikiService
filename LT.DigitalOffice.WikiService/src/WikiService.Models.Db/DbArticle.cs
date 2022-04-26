using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.WikiService.Models.Db
{
  public class DbArticle
  {
    public const string TableName = "Articles";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbRubric Rubric { get; set; } 
    public ICollection<DbArticleFile> Files { get; set; }

    public DbArticle()
    {
      Files = new HashSet<DbArticleFile>();
    }
  }
  public class DbArticleConfiguration : IEntityTypeConfiguration<DbArticle>
  {
    public void Configure(EntityTypeBuilder<DbArticle> builder)
    {
      builder.
        ToTable(DbArticle.TableName);

      builder.
        HasKey(a => a.Id);

      builder.
        HasOne(r => r.Rubric).
        WithMany(a => a.Articles);

      builder.
       HasMany(af => af.Files).
       WithOne(a => a.Article);
    }
  }
}