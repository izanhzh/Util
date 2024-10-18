using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Util.Data.EntityFrameworkCore;

#pragma warning disable EF1001
/// <summary>
/// Util自定义EntityQueryProvider
/// </summary>
public class UtilEntityQueryProvider : EntityQueryProvider
{
    /// <summary>
    /// 
    /// </summary>
    protected UtilEfCoreCurrentUnitOfWork UtilEfCoreCurrentDbContext { get; }
    /// <summary>
    /// 当前DbContext
    /// </summary>
    protected ICurrentDbContext CurrentDbContext { get; }

    /// <summary>
    /// 初始化Util自定义EntityQueryProvider
    /// </summary>
    /// <param name="queryCompiler">查询编译器</param>
    /// <param name="utilEfCoreCurrentDbContext">获取当前DbContext帮助类</param>
    /// <param name="currentDbContext">当前DbContext</param>
    public UtilEntityQueryProvider(
        IQueryCompiler queryCompiler,
        UtilEfCoreCurrentUnitOfWork utilEfCoreCurrentDbContext,
        ICurrentDbContext currentDbContext)
        : base(queryCompiler)
    {
        UtilEfCoreCurrentDbContext = utilEfCoreCurrentDbContext;
        CurrentDbContext = currentDbContext;
    }

    /// <inheritdoc/>
    public override object Execute(Expression expression)
    {
        using (UtilEfCoreCurrentDbContext.Use(CurrentDbContext.Context as UnitOfWorkBase))
        {
            return base.Execute(expression);
        }
    }

    /// <inheritdoc/>
    public override TResult Execute<TResult>(Expression expression)
    {
        using (UtilEfCoreCurrentDbContext.Use(CurrentDbContext.Context as UnitOfWorkBase))
        {
            return base.Execute<TResult>(expression);
        }
    }

    /// <inheritdoc/>
    public override TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
    {
        using (UtilEfCoreCurrentDbContext.Use(CurrentDbContext.Context as UnitOfWorkBase))
        {
            return base.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }
}
#pragma warning restore EF1001

