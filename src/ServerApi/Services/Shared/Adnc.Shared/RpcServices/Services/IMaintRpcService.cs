﻿namespace Adnc.Shared.RpcServices.Services;

public interface IMaintRpcService : IRpcService
{
    /// <summary>
    /// 获取字典数据
    /// </summary>
    /// <param name="jwtToken">token</param>
    /// <param name="id">id</param>
    /// <returns></returns>
    [Get("/maint/dicts/{id}")]
    [Headers("Authorization: Bearer", "Cache: 2000")]
    Task<ApiResponse<DictRto>> GetDictAsync(long id);
}