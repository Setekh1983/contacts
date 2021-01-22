using Alex.Contacts.Service.Utiliites;

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
      unprocessableEntity.Value.Should().BeOfType<ResponseEnvelope>();

      var value = (ResponseEnvelope)unprocessableEntity.Value;
      
      value.Errors.First().Title.Should().Be(key);
      value.Errors.First().Message.Should().Be(message);
    }
  }
}
