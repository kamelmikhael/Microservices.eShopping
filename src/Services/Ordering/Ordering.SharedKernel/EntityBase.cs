namespace Ordering.SharedKernel;

public abstract class EntityBase : IEquatable<EntityBase>
{
    public int Id { get; protected set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? LastModifiedBy { get; set; } = default!;
    public DateTime? LastModifiedAt { get; set; }

    public bool Equals(EntityBase? other)
    {
        return other?.Id == Id;
    }

    public static bool operator ==(EntityBase? left, EntityBase? right) 
    {
        if(left is null || right is null) return false;

        return left.Equals(right);
    }

    public static bool operator !=(EntityBase? left, EntityBase? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as EntityBase);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}
