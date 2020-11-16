using System;

namespace Alex.DddBasics
{
  public abstract class Entity
  {
    public Guid Id { get; }

    protected Entity() => this.Id = Guid.NewGuid();

    protected Entity(Guid guid)
    {
      if (guid == Guid.Empty)
      {
        throw new ArgumentException(Properties.Resources.EmptyGuidsAreNotAllowed);
      }
      this.Id = guid;
    }

    public override bool Equals(object obj)
    {
      if (obj is not Entity other)
      {
        return false;
      }
      if (ReferenceEquals(this, obj))
      {
        return true;
      }
      return other.Id == this.Id;
    }

    public override int GetHashCode() => HashCode.Combine(this.GetType().GetHashCode(), this.Id.GetHashCode());

    public static bool operator ==(Entity left, Entity right)
    {
      if (left is null || right is null)
      {
        return false;
      }
      return left.Equals(right);
    }
    public static bool operator !=(Entity left, Entity right)
    {
      if (left is null || right is null)
      {
        return true;
      }
      return !left.Equals(right);
    }
  }
}