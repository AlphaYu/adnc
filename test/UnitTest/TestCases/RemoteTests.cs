using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading;
using RestSharp.Serializers.SystemTextJson;
using System.Net;

namespace Adnc.UnitTest
{
    public class RemoteTests
    {
        [Fact]
        public async void GetUserInfo()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFscGhhMjAwOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhbHBoYTIwMDgiLCJlbWFpbCI6ImFscGhhMjAwOEB0b20uY29tIiwic3ViIjoiMjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiIyLCIsIm5iZiI6MTU5NjAxNjc2MiwiZXhwIjoxNTk2Mzc2NzYyLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjkwMDAifQ.EKoiTfxO6M8PF_-a0i-2BxzkQIDUAVy736mZtzcUPJ8";
            var client = new RestClient("http://localhost:8888");
            client.UseSystemTextJson();
            //var request = new RestRequest("sys/session", Method.POST);
            //request.AddJsonBody(new { account="alpha2008", password= "alpha2008" });
            //var reps=client.Execute(request);

            var request = new RestRequest("sys/session", Method.GET);
            client.Authenticator = new JwtAuthenticator(token);
            var reps = client.Execute(request);
            if(reps.StatusCode==HttpStatusCode.OK)
            {
                var result = reps.Content;
            }

        }
    }

    public interface IAccountApi
    {

    }
}
