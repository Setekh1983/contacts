using Alex.DddBasics;

using System;

namespace Alex.Contacts
{
  public class ContactNameCorrectedV1 : DomainEvent
  {
    public Guid ContactId { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public ContactNameCorrectedV1(Guid contactId, string firstName, string lastName)
    {
      this.ContactId = contactId;
      this.LastName = lastName;
      this.FirstName = firstName;
    }
  }
}