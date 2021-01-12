using CSharpFunctionalExtensions;

using System.Collections.Generic;

namespace Alex.Contacts
{
  public sealed class Name : ValueObject
  {
    private Name(string firstName, string lastName)
    {
      this.FirstName = firstName;
      this.LastName = lastName;
    }

    public string FirstName { get; }
    public string LastName { get; }

    public override string ToString()
      => string.Concat(this.FirstName, " ", this.LastName);

    public static implicit operator string(Name name)
     => (name is null) ? string.Empty : name.ToString();

    public static Result<Name> Create(string firstName, string lastName)
    {
      if (string.IsNullOrWhiteSpace(firstName))
      {
        return Result.Failure<Name>(Properties.Resources.ForenameIsMissing);
      }
      if (string.IsNullOrWhiteSpace(lastName))
      {
        return Result.Failure<Name>(Properties.Resources.SurnameIsMissing);
      }
      firstName = firstName.Trim();
      lastName = lastName.Trim();

      return Result.Success(new Name(firstName, lastName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return this.FirstName;
      yield return this.LastName;
    }
  }
}