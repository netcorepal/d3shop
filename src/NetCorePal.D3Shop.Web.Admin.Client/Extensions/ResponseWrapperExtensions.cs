using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.NewtonsoftJson;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Admin.Client.Extensions
{
    public static class ResponseWrapperExtensions
    {
        public static async Task<ResponseData<T>> WrapResponse<T>(this HttpResponseMessage responseMessage)
        {
            var responseObject = await responseMessage.Content.ReadFromNewtonsoftJsonAsync<ResponseData<T>>();
            if (responseObject is null)
                throw new KnownException("response is empty");

            return responseObject;
        }

        public static async Task<ResponseData> WrapResponse(this HttpResponseMessage responseMessage)
        {
            var responseObject = await responseMessage.Content.ReadFromNewtonsoftJsonAsync<ResponseData>();
            if (responseObject is null)
                throw new KnownException("response is empty");

            return responseObject;
        }
    }
}
