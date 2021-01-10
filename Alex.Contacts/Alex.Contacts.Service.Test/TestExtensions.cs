using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;

namespace Alex.Contacts.Service.Test
{
  static class TestExtensions
  {
    public static void ShouldBeUnprocessableEntityResult(this ActionResult result, string key, string message)
    {
      result.Should().BeOfType<UnprocessableEntityObjectResult>();

      var unprocessableEntity = (UnprocessableEntityObjectResult)result;

      unprocessableEntity.Value.Should().NotBeNull();
      unprocessableEntity.Value.Should().BeOfType<SerializableError>();

      var value = (SerializableError)unprocessableEntity.Value;

      value.First().Key.Should().Be(key);
      ((IEnumerable<string>)value.First().Value).First().Should().Be(message);
    }
  }
}
