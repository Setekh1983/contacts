using Alex.Contacts.Service.Utiliites;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alex.Contacts.Service.Test
{
  static class ContactsServiceTestExtensions
  {
#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    public static Guid GetId(this CreatedResult result)
    {
      var envelope = (ResponseEnvelope)result.Value;
      var resultType = envelope.Result.GetType();
      var idProperty = resultType.GetProperty("Id");

      return (Guid)idProperty.GetValue(envelope.Result);
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8605 // Unboxing a possibly null value.
  }
}
