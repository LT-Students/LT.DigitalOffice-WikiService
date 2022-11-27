using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.File;
using LT.DigitalOffice.ProjectService.Broker.Publishes.Interfaces;
using MassTransit;

namespace LT.DigitalOffice.WikiService.Broker.Publishes
{
  public class Publish : IPublish
  {
    private readonly IBus _bus;

    public Publish(IBus bus)
    {
      _bus = bus;
    }

    public Task RemoveFilesAsync(List<Guid> filesIds)
    {
      return _bus.Publish<IRemoveFilesPublish>(IRemoveFilesPublish.CreateObj(FileSource.Wiki, filesIds));
    }
  }
}
