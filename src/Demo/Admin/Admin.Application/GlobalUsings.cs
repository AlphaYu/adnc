﻿global using Adnc.Demo.Admin.Application.Cache;
global using Adnc.Demo.Admin.Application.Contracts.Dtos;
global using Adnc.Demo.Admin.Application.Contracts.Services;
global using Adnc.Demo.Admin.Repository;
global using Adnc.Demo.Admin.Repository.Entities;
global using Adnc.Demo.Const.Caching.Admin;
global using Adnc.Demo.Remote.Grpc.Messages;
global using Adnc.Infra.Helper;
global using Adnc.Infra.IdGenerater.Yitter;
global using Adnc.Infra.Redis;
global using Adnc.Infra.Redis.Caching;
global using Adnc.Infra.Redis.Caching.Configurations;
global using Adnc.Infra.Repository;
global using Adnc.Shared;
global using Adnc.Shared.Application.BloomFilter;
global using Adnc.Shared.Application.Caching;
global using Adnc.Shared.Application.Channels;
global using Adnc.Shared.Application.Contracts.Dtos;
global using Adnc.Shared.Application.Contracts.ResultModels;
global using Adnc.Shared.Application.Registrar;
global using Adnc.Shared.Application.Services;
global using Adnc.Shared.Repository.DapperEntities;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using System.Linq.Expressions;
global using System.Net;
global using System.Reflection;
