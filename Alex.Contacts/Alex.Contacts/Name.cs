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

    public static Result<Name> Create(string forename, string surname)
    {
      if (string.IsNullOrWhiteSpace(forename))
      {
        return Result.Failure<Name>(Properties.Resources.ForenameIsMissing);
      }
      if (string.IsNullOrWhiteSpace(surname))
      {
        return Result.Failure<Name>(Properties.Resources.SurnameIsMissing);
      }
      forename = forename.Trim();
      surname = surname.Trim();

      return Result.Success(new Name(forename, surname));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return this.FirstName;
      yield return this.LastName;
    }
  }
}