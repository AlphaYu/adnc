using System;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.Application.Contracts.Dtos
{
    /// <summary>
    /// Nlog日志
    /// </summary>
    public class NlogLogDto : MongoDto
    {
        public DateTime Date { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Exception { get; set; }

        public int ThreadID { get; set; }

        public string ThreadName { get; set; }

        public int ProcessID { get; set; }

        public string ProcessName { get; set; }

        public NlogLogPropertyDto Properties { get; set; }
    }

    public class NlogLogPropertyDto
    {
        public string TraceIdentifier { get; set; }

        public string ConnectionId { get; set; }

        public string EventId_Id { get; set; }

        public string EventId_Name { get; set; }

        public string EventId { get; set; }

        public string RemoteIpAddress { get; set; }

        public string BaseDir { get; set; }

        public string QueryUrl { get; set; }

        public string RequestMethod { get; set; }

        public string Controller { get; set; }

        public string Method { get; set; }

        public string FormContent { get; set; }

        public string QueryContent { get; set; }
    }
}
