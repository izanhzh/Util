﻿using Util.Ui.Configs;
using Util.Ui.NgZorro.Components.Tables.Configs;

namespace Util.Ui.NgZorro.Components.Tables.Helpers; 

/// <summary>
/// 表格头服务
/// </summary>
public class TableHeadService {
    /// <summary>
    /// 配置
    /// </summary>
    private readonly Config _config;
    /// <summary>
    /// 表格头共享配置
    /// </summary>
    private TableHeadShareConfig _shareConfig;
    /// <summary>
    /// 表格共享配置
    /// </summary>
    private readonly TableShareConfig _tableShareConfig;

    /// <summary>
    /// 初始化表格头服务
    /// </summary>
    /// <param name="config">配置</param>
    public TableHeadService( Config config ) {
        _config = config;
        _tableShareConfig = GetTableShareConfig();
    }

    /// <summary>
    /// 获取表格共享配置
    /// </summary>
    private TableShareConfig GetTableShareConfig() {
        return _config.GetValueFromItems<TableShareConfig>() ?? new TableShareConfig();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init() {
        CreateTableHeadShareConfig();
        CancelAutoCreateHead();
    }

    /// <summary>
    /// 创建表格头共享配置
    /// </summary>
    private void CreateTableHeadShareConfig() {
        _shareConfig = new TableHeadShareConfig();
        _config.SetValueToItems( _shareConfig );
    }

    /// <summary>
    /// 取消自动创建表头
    /// </summary>
    private void CancelAutoCreateHead() {
        _tableShareConfig.IsAutoCreateHead = false;
    }
}