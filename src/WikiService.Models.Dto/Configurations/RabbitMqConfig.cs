using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.File;

namespace LT.DigitalOffice.WikiService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string CheckArticlesExistenceEndpoint { get; set; }
    public string CreateFilesEndpoint { get; set; }

    // file

    [AutoInjectRequest(typeof(IGetFilesRequest))]
    public string GetFilesEndpoint { get; set; }
  }
}
