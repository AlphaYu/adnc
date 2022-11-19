﻿namespace Adnc.Shared.Consts.Caching.Maint;

public class CachingConsts : Common.CachingConsts
{
    public const string DictPreheatedKey = "adnc:maint:dicts:preheated";
    public const string CfgPreheatedKey = "adnc:maint:cfgs:preheated";

    public const string DictSingleKeyPrefix = "adnc:maint:dicts:single";
    public const string CfgSingleKeyPrefix = "adnc:maint:cfgs:single";

    public const string BloomfilterOfCacheKey = "adnc:maint:bloomfilter:cachekeys";
}