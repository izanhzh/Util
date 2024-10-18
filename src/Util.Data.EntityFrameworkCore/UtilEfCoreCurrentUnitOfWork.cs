namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// 当前工作单元
/// </summary>
public class UtilEfCoreCurrentUnitOfWork
{
    private readonly AsyncLocal<UnitOfWorkBase> _current = new AsyncLocal<UnitOfWorkBase>();

    /// <summary>
    /// 当前工作单元值
    /// </summary>
    public UnitOfWorkBase Value => _current.Value;

    /// <summary>
    /// 实现using，Disposable后自动恢复为原先的UnitOfWork
    /// </summary>
    /// <param name="unitOfWork">工作单元</param>
    /// <returns></returns>
    public IDisposable Use(UnitOfWorkBase unitOfWork)
    {
        var previousValue = Value;
        _current.Value = unitOfWork;
        return new DisposeAction(() =>
        {
            _current.Value = previousValue;
        });
    }
}

