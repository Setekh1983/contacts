using CSharpFunctionalExtensions;

namespace Alex.Addresses
{
  public sealed class Name : SimpleValueObject<string>
  {
    private Name(string value)
      :base(value)
    {
    }

    public static implicit operator string(Name name)
     => (name is null) ? string.Empty : name.Value;

    public static Result<Name> Create(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return Result.Failure<Name>(Properties.Resources.MissingNameValue);
      }
      name = name.Trim();
      
      return Result.Success(new Name(name));
    }
  }
}