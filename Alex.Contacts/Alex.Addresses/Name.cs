﻿using CSharpFunctionalExtensions;

using System.Collections.Generic;

namespace Alex.Addresses
{
  public class Name : ValueObject
  {
    private Name(string forename, string surname)
    {
      this.Forename = forename;
      this.Surname = surname;
    }

    public string Forename { get; }
    public string Surname { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return this.Forename;
      yield return this.Surname;
    }

    public override string ToString()
      => string.Concat(this.Forename, " ", this.Surname);

    public static implicit operator string(Name name)
    {
      if (name is null)
      {
        return string.Empty;
      }
      return name.ToString();
    }

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
  }
}