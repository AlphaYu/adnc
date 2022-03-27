﻿using AttributeUsageAttribute = System.AttributeUsageAttribute;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute : AuthorizeAttribute
{
    public string[] Codes { get; private set; }

    public PermissionAttribute(params string[] codes)
        : base(AuthorizePolicy.Default)
        => Codes = codes;
}