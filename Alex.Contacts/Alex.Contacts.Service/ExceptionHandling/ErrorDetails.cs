namespace Alex.Contacts.Service.ExceptionHandling
{
  public static partial class ExceptionHandlerMiddlewareExtensions
  {
    class ErrorDetails
    {
      public ErrorDetails(int statusCode, string error)
      {
        this.StatusCode = statusCode;
        this.Error = error;
      }
      public int StatusCode { get; }
      public string Error { get; }
    }

  }
}
