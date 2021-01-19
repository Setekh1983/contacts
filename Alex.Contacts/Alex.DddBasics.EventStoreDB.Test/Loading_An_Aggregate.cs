using Alex.DddBasics.EventStoreDB.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.EventStoreDB.Test
{
  [TestClass]
  public class Loading_An_Aggregate : RepositoryTest
  {
    [TestMethod]
    public void Restores_The_Current_State()
    {
      var handlerResult = new DomainEventHandlerStub();
      IDomainEventDispatcher domainEventDispatcher = new DomainEventDispatcher(handlerResult.CreateHandler);

      var homer = new Citizen();
      var marge = new Citizen();

      homer.Marry(marge);
      var address = new Address("Springfield", "12345", "Evergreen Terrace", "7890", "USA");
      homer.Move(address);

      var repo = new Repository<Citizen>(GetEventStoreClient(), GetEventTypeMap(), domainEventDispatcher);
      repo.SaveAsync(homer).GetAwaiter().GetResult();

      repo = new Repository<Citizen>(GetEventStoreClient(), GetEventTypeMap(), domainEventDispatcher);
      Citizen sut = repo.LoadAsync(homer.Id).GetAwaiter().GetResult();

      sut.Should().NotBeNull();
      sut.Id.Should().Be(homer.Id);
      sut.GetChanges().Should().BeEmpty();
      sut.Partner.Should().Be(marge.Id);
      sut.Address.Should().Be(address);
      ((IPersistableAggregate)sut).OriginatingVersion.Should().Be(1);
    }

    [TestMethod]
    public void With_An_Unknown_Id_Returns_Null()
    {
      var handlerResult = new DomainEventHandlerStub();
      IDomainEventDispatcher domainEventDispatcher = new DomainEventDispatcher(handlerResult.CreateHandler);
      var sut = new Repository<Citizen>(GetEventStoreClient(), GetEventTypeMap(), domainEventDispatcher);

      var citizen = sut.LoadAsync(Guid.NewGuid()).GetAwaiter().GetResult();

      citizen.Should().BeNull();
    }
  }
}
