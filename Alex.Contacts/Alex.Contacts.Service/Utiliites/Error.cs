namespace Alex.Contacts.Service.Utiliites
{
  public class Error
  {
    public Error(string title, string message)
    {
      this.Title = title;
      this.Message = message;
    }

    public string Title { get; }
    public string Message { get; }
  }
}