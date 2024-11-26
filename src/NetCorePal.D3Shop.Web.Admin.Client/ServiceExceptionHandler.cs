namespace NetCorePal.D3Shop.Web.Admin.Client;

public class ServiceExceptionHandler(MessageService messageService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await base.SendAsync(request, cancellationToken);
            result.EnsureSuccessStatusCode();

            return result;
        }
        catch (Exception)
        {
            _ = messageService.Error("服务内部异常！");
            throw;
        }
    }
}