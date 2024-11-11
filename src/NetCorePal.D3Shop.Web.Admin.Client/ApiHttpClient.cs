using System.Net.Http.Headers;
using NetCorePal.D3Shop.Web.Admin.Client.Auth;
using NetCorePal.D3Shop.Web.Admin.Client.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.NewtonsoftJson;

namespace NetCorePal.D3Shop.Web.Admin.Client;

public class ApiHttpClient(
    HttpClient httpClient,
    IAccessTokenProvider accessTokenProvider)
{
    private async Task SetAuthorizeHeader()
    {
        var accessToken = await accessTokenProvider.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
    }

    public async Task<ResponseData<T>> GetWithDataAsync<T>(string path)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.GetAsync(path);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse<T>();
        return default!;
    }

    public async Task<ResponseData<T1>> PostWithDataAsync<T1, T2>(string path, T2 postModel)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.PostAsNewtonsoftJsonAsync(path, postModel);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse<T1>();
        return default!;
    }

    public async Task<ResponseData<T1>> PutWithDataAsync<T1, T2>(string path, T2 postModel)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.PutAsNewtonsoftJsonAsync(path, postModel);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse<T1>();
        return default!;
    }

    public async Task<ResponseData<T>> DeleteWithDataAsync<T>(string path)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.DeleteAsync(path);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse<T>();
        return default!;
    }

    public async Task<ResponseData> GetAsync(string path)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.GetAsync(path);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse();
        return default!;
    }

    public async Task<ResponseData> PostAsync<T>(string path, T postModel)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.PostAsNewtonsoftJsonAsync(path, postModel);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse();
        return default!;
    }

    public async Task<ResponseData> PutAsync<T>(string path, T postModel)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.PutAsNewtonsoftJsonAsync(path, postModel);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse();
        return default!;
    }

    public async Task<ResponseData> DeleteAsync(string path)
    {
        await SetAuthorizeHeader();
        var response = await httpClient.DeleteAsync(path);
        if (response.IsSuccessStatusCode)
            return await response.WrapResponse();
        return default!;
    }
}