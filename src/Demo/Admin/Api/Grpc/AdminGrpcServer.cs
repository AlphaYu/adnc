using Adnc.Demo.Remote.Grpc.Messages;
using Adnc.Demo.Remote.Grpc.Services;
using Grpc.Core;

namespace Adnc.Demo.Admin.Api.Grpc;

public class AdminGrpcServer(ISysConfigService sysConfigService, IDictService dictService/*,IObjectMapper mapper*/) : AdminGrpc.AdminGrpcBase
{
    public override async Task<SysConfigSimpleListReply> GetSysConfigList(SysConfigSimpleRequest request, ServerCallContext context)
    {
        var replyList = new SysConfigSimpleListReply();
        var dtos = await sysConfigService.GetListAsync(request.Keys);
        if (dtos is null)
        {
            return replyList;
        }
        else
        {
            var reply = dtos.Select(x => new SysConfigSimpleReply { Name = x.Name, Value = x.Value, Key = x.Key });
            replyList.List.AddRange(reply);
            return replyList;
        }
        //var reply = dtos is null ? [] : mapper.Map<List<SysConfigSimpleReply>>(dtos);
        //replyList.List.AddRange(reply);
        //return replyList;
    }

    public override async Task<DictOptionListReply> GetDictOptions(DictOptionRequest request, ServerCallContext context)
    {
        var replyList = new DictOptionListReply();
        var dtos = await dictService.GetOptionsAsync(request.Codes);
        if (dtos is null)
        {
            return replyList;
        }
        else
        {
            foreach (var dto in dtos)
            {
                var reply = new DictOptionReply() { Name = dto.Name, Code = dto.Code };
                reply.DictDataList.AddRange(dto.DictDataList.Select(x => new DictOptionReply.Types.DataOption { Label = x.Label, Value = x.Value, TagType = x.TagType }));
                replyList.List.Add(reply);
            }
            return replyList;
        }
        //var reply = dtos is null ? [] : mapper.Map<List<DictOptionReply>>(dtos);
        //replyList.List.AddRange(reply);
        //return replyList;
    }
}
