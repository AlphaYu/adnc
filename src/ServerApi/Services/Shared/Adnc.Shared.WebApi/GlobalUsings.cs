﻿global using Adnc.Infra.Caching;
global using Adnc.Infra.Consul;
global using Adnc.Infra.Consul.Consumer;
global using Adnc.Infra.Core;
global using Adnc.Infra.EventBus.RabbitMq;
global using Adnc.Infra.Helper;
global using Adnc.Infra.Mongo;
global using Adnc.Infra.Mongo.Configuration;
global using Adnc.Infra.Mongo.Extensions;
global using Adnc.Shared.ConfigModels;
global using Adnc.Shared.ResultModels;
global using Adnc.Shared.RpcServices;
global using Adnc.Shared.RpcServices.Services;
global using Adnc.Shared.WebApi;
global using Adnc.Shared.WebApi.Extensions;
global using Adnc.Shared.WebApi.Middleware;
global using Autofac;
global using DotNetCore.CAP;
global using FluentValidation.AspNetCore;
global using HealthChecks.UI.Client;
global using LiteX.HealthChecks.Redis;
global using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Logging;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using MongoDB.Driver;
global using Newtonsoft.Json.Linq;
global using Polly;
global using Polly.Timeout;
global using Refit;
global using SkyApm.Diagnostics.CAP;
global using SkyApm.Utilities.DependencyInjection;
global using System;
global using System.Collections.Generic;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Threading;
global using System.Threading.Tasks;