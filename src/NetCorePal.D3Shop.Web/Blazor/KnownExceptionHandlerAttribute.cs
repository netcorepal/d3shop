using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using Rougamo;
using Rougamo.Context;

namespace NetCorePal.D3Shop.Web.Blazor;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class KnownExceptionHandlerAttribute : MoAttribute
{
    public override void OnException(MethodContext context)
    {
        // 检查异常是否是 IKnownException 类型
        if (context.Exception is not IKnownException ex) return;

        // 获取方法的返回类型
        var returnType = context.ReturnType;
        if (returnType is null) return;

        // 处理非泛型 ResponseData 和 Task<ResponseData>
        if (returnType == typeof(ResponseData) || returnType == typeof(Task<ResponseData>))
        {
            context.HandledException(this, new ResponseData(
                success: false,
                message: ex.Message,
                code: ex.ErrorCode,
                errorData: ex.ErrorData
            ));
            return;
        }

        switch (returnType.IsGenericType)
        {
            // 处理泛型 ResponseData<T> 
            case true when returnType.GetGenericTypeDefinition() == typeof(ResponseData<>):
            {
                var genericArgument = returnType.GetGenericArguments()[0];
                var responseInstance = Activator.CreateInstance(returnType,
                    Activator.CreateInstance(genericArgument),
                    false, ex.Message, ex.ErrorCode, ex.ErrorData);

                context.HandledException(this, responseInstance!);
                return;
            }
            // 处理泛型 Task<ResponseData<T>>
            case true when returnType.GetGenericTypeDefinition() == typeof(Task<>):
            {
                var taskArgument = returnType.GetGenericArguments()[0];

                if (taskArgument.IsGenericType && taskArgument.GetGenericTypeDefinition() == typeof(ResponseData<>))
                {
                    var responseInstance = Activator.CreateInstance(taskArgument,
                        default, false, ex.Message, ex.ErrorCode, ex.ErrorData);

                    context.HandledException(this, responseInstance!);
                }

                break;
            }
        }
    }
}