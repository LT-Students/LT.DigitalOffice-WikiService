using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.EndpointSupport.Broker.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Auth;
using LT.DigitalOffice.Models.Broker.Requests.Rights;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Requests.File;

namespace LT.DigitalOffice.WikiService.Models.Dto.Configurations
{
  public class RabbitMqConfig : ExtendedBaseRabbitMqConfig
  {
    //AuthService
    [AutoInjectRequest(typeof(IGetTokenRequest))]
    public string GetTokenEndpoint { get; set; }

    //RightsService
    [AutoInjectRequest(typeof(IGetUserRolesRequest))]
    public string GetUserRolesEndpoint { get; set; }

    //UserService
    [AutoInjectRequest(typeof(IGetUserDataRequest))]
    public string GetUserDataEndpoint { get; set; }
  }
}
