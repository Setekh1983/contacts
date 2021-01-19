using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Initializing_Event_Dispatching
  {
    [TestMethod]
    public void Using_Dependency_Injection_With_One_Handler()
    {
      IServiceCollection services = new ServiceCollection();

      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));
      services.AddTransient(typeof(DomainEventHandlerStub.CitizenMovedDomainEventHandler1Stub));

      IServiceProvider provider = services.BuildServiceProvider();

      var service = provider.GetService(typeof(DomainEventHandlerStub.CitizenMovedDomainEventHandler1Stub));

      service.Should().NotBeNull();
      service.Should().BeOfType<DomainEventHandlerStub.CitizenMovedDomainEventHandler1Stub>();
    }
    [TestMethod]
    public void Using_Dependency_Injection_One_Handler_Querying_Abstraction()
    {
      IServiceCollection services = new ServiceCollection();

      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));
      services.AddTransient(typeof(IDomainEventHandler<CitizenMovedEvent>), typeof(DomainEventHandlerStub.CitizenMovedDomainEventHandler1Stub));

      IServiceProvider provider = services.BuildServiceProvider();

      var service = provider.GetService(typeof(IDomainEventHandler<CitizenMovedEvent>));

      service.Should().NotBeNull();
      service.Should().BeAssignableTo<IDomainEventHandler<CitizenMovedEvent>>();
    }
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
    public void Using_UseDddBasics()
    {
      IServiceCollection services = new ServiceCollection();
      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));

      services.UseDddBasics(typeof(Citizen).Assembly);

      var provider = services.BuildServiceProvider();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

      var service = (IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>)provider.GetService(typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>));

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

      service.Should().NotBeNull();
      service.Should().HaveCount(2);
      service.First().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
      service.Last().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
    }

    [TestMethod]
    public void Using_UseDddBasics_With_Domain_Event_Handler_Assemblies()
    {
      IServiceCollection services = new ServiceCollection();
      services.AddTransient(typeof(IList<IDomainEvent>), typeof(List<IDomainEvent>));

      services.UseDddBasics(typeof(Citizen).Assembly, typeof(DomainEventHandlerStub).Assembly);

      var provider = services.BuildServiceProvider();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

      var service = (IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>)provider.GetService(typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>));

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

      service.Should().NotBeNull();
      service.Should().HaveCount(2);
      service.First().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
      service.Last().Should().BeAssignableTo<IDomainEventHandler<CitizenMarriedEvent>>();
    }

    [TestMethod]
    public void Using_UseDddBasics_With_Domain_Event_Handler_Assemblies_Null_Raises_Error()
    {
      IServiceCollection services = new ServiceCollection();

      Action action = () => services.UseDddBasics(typeof(Citizen).Assembly, null);

      action.Should().Throw<ArgumentNullException>();

    }

    [TestMethod]
    public void Using_UseDddBasics_Registers_IDomainEventDispatcher()
    {
      IServiceCollection services = new ServiceCollection();

      services.UseDddBasics(typeof(Citizen).Assembly);

      var provider = services.BuildServiceProvider();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

      var service = provider.GetService(typeof(IDomainEventDispatcher));

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

      service.Should().NotBeNull();
      service.Should().BeAssignableTo<IDomainEventDispatcher>();
    }

    [TestMethod]
    public void Using_UseDddBasics_Raises_Error_When_Assembly_Is_Null()
    {
      IServiceCollection services = new ServiceCollection();

      Action action = () => services.UseDddBasics(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
