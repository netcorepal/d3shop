using AntDesign;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using Rougamo;
using Rougamo.Context;

namespace NetCorePal.D3Shop.Web.Blazor;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ServiceExceptionHandlerAttribute : MoAttribute
{
    private readonly KnownExceptionHandleMiddlewareOptions _options = new();

    public override void OnException(MethodContext context)
    {
        if (context.Exception is IKnownException ex)
        {
            context.HandledException(this, new ResponseData(success: false, message: ex.Message, code: ex.ErrorCode,
                errorData: ex.ErrorData));
        }
        else
        {
            var logger = context.GetRequiredService<ILogger<KnownExceptionHandleMiddleware>>();
            logger.LogError(context.Exception, message: "{Message}", _options.UnknownExceptionMessage);
            var messageService = context.GetRequiredService<MessageService>();
            _ = messageService.Error("服务器内部异常！");
        }
    }
}