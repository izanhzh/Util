using Util.Domain.Entities;

namespace Util.Data.EntityFrameworkCore.Filters; 

/// <summary>
/// 逻辑删除过滤器
/// </summary>
public class DeleteFilter : FilterBase<IDelete> {
    /// <summary>
    /// 获取过滤表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public override Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class {
        var unitOfWork = state as UnitOfWorkBase;
        Expression<Func<TEntity, bool>> expression = entity => !EF.Property<bool>( entity, "IsDeleted" );
        if ( unitOfWork != null )
            expression = entity => DeletePredicate(EF.Property<bool>(entity, "IsDeleted"));
        return expression;
    }

    /// <summary>
    /// 定义一个方法，用于映射到自定义 SQL
    /// </summary>
    /// <param name="isDeleted">实体值</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static bool DeletePredicate( bool isDeleted ){
        throw new NotSupportedException();
    }
}