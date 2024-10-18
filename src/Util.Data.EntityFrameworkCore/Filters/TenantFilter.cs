namespace Util.Data.EntityFrameworkCore.Filters;

/// <summary>
/// 租户过滤器
/// </summary>
public class TenantFilter : FilterBase<ITenant> {
    /// <summary>
    /// 租户管理器
    /// </summary>
    private readonly ITenantManager _manager;

    /// <summary>
    /// 初始化租户过滤器
    /// </summary>
    /// <param name="manager">租户管理器</param>
    public TenantFilter( ITenantManager manager ) {
        _manager = manager ?? throw new ArgumentNullException( nameof( manager ) );
    }

    /// <summary>
    /// 获取过滤表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public override Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class {
        if ( _manager.Enabled() == false )
            return null;
        if ( _manager.IsDisableTenantFilter() )
            return null;
        var unitOfWork = state as UnitOfWorkBase;
        if ( unitOfWork == null )
            return null;
        Expression<Func<TEntity, bool>> expression = entity => TenantPredicate( EF.Property<string>( entity, "TenantId" ) , unitOfWork.CurrentTenantId);
        return expression;
    }

    /// <summary>
    /// 定义一个方法，用于映射到自定义 SQL
    /// </summary>
    /// <param name="tenantId">实体值</param>
    /// <param name="currentTenantId">参数值</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static bool TenantPredicate( string tenantId, string currentTenantId ){
        throw new NotSupportedException();
    }
}