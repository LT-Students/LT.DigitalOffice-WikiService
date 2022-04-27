using LT.DigitalOffice.WikiService.Data.Interfaces;
using LT.DigitalOffice.WikiService.Data.Provider;

namespace LT.DigitalOffice.WikiService.Data
{
  public class RubricRepository : IRubricRepository
  {
    private readonly IDataProvider _provider;

    public RubricRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }
  }
}