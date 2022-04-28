using System;
using LT.DigitalOffice.WikiService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.WikiService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(WikiServiceDbContext))]
  [Migration("20220427000000_InitialCreate")]
  public class InitialCreate : Migration
  {
    private void CreateTableRubrics(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbRubric.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          ParentId = table.Column<Guid>(nullable: true),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Rubrics", x => x.Id);
        });
    }

    private void CreateTableArticles(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbArticle.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          Content = table.Column<string>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          RubricId = table.Column<Guid>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Articles", x => x.Id);
        });
    }

    private void CreateTableArticlesFiles(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbArticleFile.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          ArticleId = table.Column<Guid>(nullable: false),
          FileId = table.Column<Guid>(nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_ArticlesFiles", x => x.Id);
        });
    }

    protected override void Up(MigrationBuilder migrationBuilder)
    {
      CreateTableRubrics(migrationBuilder);

      CreateTableArticles(migrationBuilder);

      CreateTableArticlesFiles(migrationBuilder);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(DbRubric.TableName);

      migrationBuilder.DropTable(DbArticle.TableName);

      migrationBuilder.DropTable(DbArticleFile.TableName);
    }
  }
}
