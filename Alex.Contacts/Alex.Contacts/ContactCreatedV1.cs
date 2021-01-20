using Alex.DddBasics;

using System;

namespace Alex.Contacts
{
  public class ContactCreatedV1 : DomainEvent
  {
    public string FirstName { get; }
    public string LastName { get; }
    public Guid ContactId { get; }

    public ContactCreatedV1(Guid contactId, string firstName, string lastName)
    {
      this.FirstName = firstName;
      this.LastName = lastName;
      this.ContactId = contactId;
    }
  }
}