using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using LT.DigitalOffice.WikiService.Broker.Publishes.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.WikiService.Broker.Publishes
{
  public class Publish : IPublish
  {
    private readonly IBus _bus;

    public Publish(
      IBus bus)
    {
      _bus = bus;
    }
    public async Task RemoveFilesAsync(List<Guid> filesIds)
    {
      await _bus.Publish<IRemoveFilesPublish>(IRemoveFilesPublish.CreateObj(
        filesIds: filesIds));
    }
  }
}
