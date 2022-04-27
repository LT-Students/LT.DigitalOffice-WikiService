using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.WikiService.Models.Db
{
  public class DbArticleFile
  {
    public const string TableName = "ArticlesFiles";

    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public Guid FileId { get; set; }

    public DbArticle Article { get; set; }
  }

  public class DbArticleFileConfiguration : IEntityTypeConfiguration<DbArticleFile>
  {
    public void Configure(EntityTypeBuilder<DbArticleFile> builder)
    {
      builder
        .ToTable(DbArticleFile.TableName);

      builder
        .HasKey(af => af.Id);

      builder
        .HasOne(a => a.Article)
        .WithMany(af => af.Files);
    }
  }
}
