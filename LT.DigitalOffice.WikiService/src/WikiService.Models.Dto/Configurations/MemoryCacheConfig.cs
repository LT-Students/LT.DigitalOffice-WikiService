namespace LT.DigitalOffice.WikiService.Models.Dto.Configurations
{
  public record MemoryCacheConfig
  {
    public const string SectionName = "MemoryCache";
    public double CacheLiveInMinutes { get; set; }
  }
}