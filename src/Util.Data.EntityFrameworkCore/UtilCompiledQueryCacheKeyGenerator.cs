using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Util自定义EFCore编译查询缓存Key生产器
/// </summary>
public class UtilCompiledQueryCacheKeyGenerator : ICompiledQueryCacheKeyGenerator
{
    /// <summary>
    /// 原生的EFCore编译查询缓存Key生产器
    /// </summary>
    protected ICompiledQueryCacheKeyGenerator InnerCompiledQueryCacheKeyGenerator { get; }
    /// <summary>
    /// 当前DbContext
    /// </summary>
    protected ICurrentDbContext CurrentContext { get; }

    /// <summary>
    /// 初始化EFCore编译查询缓存Key生产器
    /// </summary>
    /// <param name="innerCompiledQueryCacheKeyGenerator">原生的EFCore编译查询缓存Key生产器</param>
    /// <param name="currentContext">当前DbContext</param>
    public UtilCompiledQueryCacheKeyGenerator(
        ICompiledQueryCacheKeyGenerator innerCompiledQueryCacheKeyGenerator,
        ICurrentDbContext currentContext)
    {
        InnerCompiledQueryCacheKeyGenerator = innerCompiledQueryCacheKeyGenerator;
        CurrentContext = currentContext;
    }

    /// <inheritdoc/>
    public virtual object GenerateCacheKey(Expression query, bool async)
    {
        var cacheKey = InnerCompiledQueryCacheKeyGenerator.GenerateCacheKey(query, async);
        if (CurrentContext.Context is UnitOfWorkBase unitOfWorkBase)
        {
            return new UtilCompiledQueryCacheKey(cacheKey, unitOfWorkBase.GetCompiledQueryCacheKey());
        }

        return cacheKey;
    }

    private readonly struct UtilCompiledQueryCacheKey : IEquatable<UtilCompiledQueryCacheKey>
    {
        private readonly object _compiledQueryCacheKey;
        private readonly string _currentFilterCacheKey;

        public UtilCompiledQueryCacheKey(object compiledQueryCacheKey, string currentFilterCacheKey)
        {
            _compiledQueryCacheKey = compiledQueryCacheKey;
            _currentFilterCacheKey = currentFilterCacheKey;
        }

        public override bool Equals(object obj)
        {
            return obj is UtilCompiledQueryCacheKey key && Equals(key);
        }

        public bool Equals(UtilCompiledQueryCacheKey other)
        {
            return _compiledQueryCacheKey.Equals(other._compiledQueryCacheKey) &&
                   _currentFilterCacheKey == other._currentFilterCacheKey;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_compiledQueryCacheKey, _currentFilterCacheKey);
        }
    }
}
