using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Adnc.Common.Models;

namespace Adnc.Common
{
    public class BusinessException : Exception
    {
        public BusinessException(ErrorModel errorModel)
            : base(JsonConvert.SerializeObject(errorModel, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }))
        {
            base.HResult = (int)errorModel.StatusCode;
        }
    }
}
