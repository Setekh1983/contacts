namespace Alex.Contacts.Service
{
  public class CreateContactCommand
  {
    public string LastName;

    public CreateContactCommand()
    {
    }

    public string Forename { get; set; }
  }
}