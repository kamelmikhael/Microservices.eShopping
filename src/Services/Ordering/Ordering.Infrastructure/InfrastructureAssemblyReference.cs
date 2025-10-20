using System.Reflection;

namespace Ordering.Infrastructure;

public static class InfrastructureAssemblyReference
{
    public static readonly Assembly Assembly = typeof(InfrastructureAssemblyReference).Assembly;
}
