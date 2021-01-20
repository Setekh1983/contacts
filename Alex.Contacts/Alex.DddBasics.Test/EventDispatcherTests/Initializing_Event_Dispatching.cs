using Alex.DddBasics.DependencyInjection;
using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Initializing_Event_Dispatching
  {
    [TestMethod]
    public void Using_Dependency_Injection_Querying_All_Abstractions()
    {
      IServiceCollection services = new ServiceCollection();

      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));
      services.AddTransient(typeof(IDomainEventHandler<CitizenMarriedEvent>), typeof(DomainEventHandlerStub.CitizenMarriedDomainEventHandler1Stub));
      services.AddTransient(typeof(IDomainEventHandler<CitizenMarriedEvent>), typeof(DomainEventHandlerStub.CitizenMarriedDomainEventHandler2Stub));

      IServiceProvider provider = services.BuildServiceProvider();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

      var service = (IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>)provider.GetService(typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>));

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

      service.Should().NotBeNull();
      service.Should().HaveCount(2);
      service.First().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
      service.Last().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
    }
    [TestMethod]
    public void Using_AddDddBasics()
    {
      IServiceCollection services = new ServiceCollection();
      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));

      services.AddDddBasics(typeof(Citizen).Assembly);

      var provider = services.BuildServiceProvider();
      var service = (IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>)provider.GetService(typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>));

      service.Should().NotBeNull();
      service.Should().HaveCount(2);
      service.First().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
      service.Last().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
    }

    [TestMethod]
    public void Using_AddDddBasics_With_Domain_Event_Handler_Assemblies()
    {
      IServiceCollection services = new ServiceCollection();
      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));

      services.AddDddBasics(typeof(Citizen).Assembly, typeof(DomainEventHandlerStub).Assembly);

      var provider = services.BuildServiceProvider();
      var service = (IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>)provider.GetService(typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>));

      service.Should().NotBeNull();
      service.Should().HaveCount(2);
      service.First().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
      service.Last().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
    }

    [TestMethod]
    public void Using_AddDddBasics_With_Domain_Event_Handler_Assemblies_Null_Raises_Error()
    {
      IServiceCollection services = new ServiceCollection();

      Action action = () => services.AddDddBasics(typeof(Citizen).Assembly, null);

      action.Should().Throw<ArgumentNullException>();

    }

    [TestMethod]
    public void Using_AddDddBasics_Registers_IDomainEventDispatcher()
    {
      IServiceCollection services = new ServiceCollection();

      services.AddDddBasics(typeof(Citizen).Assembly);

      var provider = services.BuildServiceProvider();
      var service = provider.GetService(typeof(IDomainEventDispatcher));

      service.Should().NotBeNull();
      service.Should().BeAssignableTo<IDomainEventDispatcher>();
    }

    [TestMethod]
    public void Using_AddDddBasics_Raises_Error_When_Assembly_Is_Null()
    {
      IServiceCollection services = new ServiceCollection();

      Action action = () => services.AddDddBasics(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
