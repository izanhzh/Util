using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Util自定义DbContextOptionsExtension
/// </summary>
public class UtilDbContextOptionsExtension : IDbContextOptionsExtension
{
    /// <inheritdoc/>
    public DbContextOptionsExtensionInfo Info => new UtilOptionsExtensionInfo(this);

    /// <inheritdoc/>
    public void ApplyServices(IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ICompiledQueryCacheKeyGenerator));
        if (serviceDescriptor != null && serviceDescriptor.ImplementationType != null)
        {
            services.Remove(serviceDescriptor);
            services.AddScoped(serviceDescriptor.ImplementationType);
            services.Add(ServiceDescriptor.Scoped<ICompiledQueryCacheKeyGenerator>(provider => ActivatorUtilities.CreateInstance<UtilCompiledQueryCacheKeyGenerator>(provider, (ICompiledQueryCacheKeyGenerator)provider.GetRequiredService(serviceDescriptor.ImplementationType))));
        }

        services.Replace(ServiceDescriptor.Scoped<IAsyncQueryProvider, UtilEntityQueryProvider>());
        services.AddSingleton<UtilEfCoreCurrentUnitOfWork>();
    }

    /// <inheritdoc/>
    public void Validate(IDbContextOptions options)
    {
    }

    private class UtilOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public UtilOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => other is UtilOptionsExtensionInfo;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override string LogFragment => "UtilOptionsExtension";
    }
}

