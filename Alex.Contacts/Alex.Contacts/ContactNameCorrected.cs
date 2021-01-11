using Alex.DddBasics;

using System;

namespace Alex.Contacts
{
  public class ContactNameCorrected : IDomainEvent
  {
    public Guid ContactId { get; }
    public string Forename { get; }
    public string LastName { get; }

    public ContactNameCorrected(Guid contactId, string forename, string lastName)
    {
      this.ContactId = contactId;
      this.LastName = lastName;
      this.Forename = forename;
    }
  }
}