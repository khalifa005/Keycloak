using KeycloakBasedOnOpenApi.Helper;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

namespace KeycloakBasedOnOpenApi.Services
{
  public class HttpRequestHelper
  {
    private readonly HttpClient _http;
    public HttpRequestHelper(HttpClient http)
    {
      _http = http;
    }

    public async Task<ApiResponse<T>> GetRequestAsync<T>(string Endpoint, string token, bool returnedAsArrayByte = false) where T : class
    {
      var apiResponse = new ApiResponse<T>((int)HttpStatusCode.OK);

      HttpRequestMessage httpRequestMessage = new();

      httpRequestMessage.Method = new HttpMethod(HttpMethod.Get.ToString());

      httpRequestMessage.RequestUri = new Uri(Endpoint);


      if (!string.IsNullOrEmpty(token))
      {
        httpRequestMessage.Headers.Authorization
           = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
      }

      var result = await _http.SendAsync(httpRequestMessage);

      dynamic content = null;

      if (returnedAsArrayByte)
        content = await result.Content.ReadAsByteArrayAsync();
      else
        content = await result.Content.ReadAsStringAsync();



      if (result.StatusCode == HttpStatusCode.InternalServerError ||
         result.StatusCode == HttpStatusCode.BadRequest ||
         result.StatusCode == HttpStatusCode.Unauthorized)
      {
        Console.WriteLine(content);

        apiResponse.StatusCode = (int)result.StatusCode;
        return apiResponse;
      }


      try
      {
        dynamic res = null;
        if (returnedAsArrayByte)
          res = content;
        else
          res = JsonConvert.DeserializeObject<T>(content);

        apiResponse.Data = res;

        return apiResponse;

      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message + e.StackTrace);
        Console.WriteLine(content);
        apiResponse.ErrorMessage = e.Message;

        return apiResponse;

      }
    }

    public async Task<ApiResponse<T>> PostRequestAsync<T>(string Endpoint, object content, string token) where T : class
    {
      var apiResponse = new ApiResponse<T>((int)HttpStatusCode.OK);

      HttpRequestMessage httpRequestMessage = new();

      httpRequestMessage.Method = new HttpMethod(HttpMethod.Post.ToString());

      httpRequestMessage.RequestUri = new Uri(Endpoint);

      var serializedObject = JsonConvert.SerializeObject(content);

      httpRequestMessage.Content = new StringContent(serializedObject);


      httpRequestMessage.Content.Headers.ContentType
          = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json);


      if (!string.IsNullOrEmpty(token))
      {
        httpRequestMessage.Headers.Authorization
       = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
      }

      var result = await _http.SendAsync(httpRequestMessage);


      var responde = await result.Content.ReadAsStringAsync();


      if (result.StatusCode == HttpStatusCode.InternalServerError ||
          result.StatusCode == HttpStatusCode.BadRequest ||
          result.StatusCode == HttpStatusCode.Unauthorized)
      {
        apiResponse.StatusCode = (int)result.StatusCode;

        return apiResponse;
      }

      try
      {
        var res = JsonConvert.DeserializeObject<T>(responde);

        apiResponse.Data = res;

        return apiResponse;
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message + e.StackTrace);
        Console.WriteLine(responde);

        apiResponse.ErrorMessage = e.Message;

        return apiResponse;
      }
    }

    public async Task<ApiResponse<T>> DeleteRequestAsync<T>(string Endpoint, string token) where T : class
    {
      var apiResponse = new ApiResponse<T>((int)HttpStatusCode.OK);

      HttpRequestMessage httpRequestMessage = new();

      httpRequestMessage.Method = new HttpMethod(HttpMethod.Delete.ToString());

      httpRequestMessage.RequestUri = new Uri(Endpoint);


      if (!string.IsNullOrEmpty(token))
      {
        httpRequestMessage.Headers.Authorization
           = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
      }

      var result = await _http.SendAsync(httpRequestMessage);

      dynamic content = null;
      content = await result.Content.ReadAsStringAsync();



      if (result.StatusCode == HttpStatusCode.InternalServerError ||
         result.StatusCode == HttpStatusCode.BadRequest ||
         result.StatusCode == HttpStatusCode.Unauthorized)
      {
        Console.WriteLine(content);

        apiResponse.StatusCode = (int)result.StatusCode;
        return apiResponse;
      }


      try
      {

        return apiResponse;

      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message + e.StackTrace);
        Console.WriteLine(content);
        apiResponse.ErrorMessage = e.Message;

        return apiResponse;

      }
    }

    public async Task<ApiResponse<T>> UpdateRequestAsync<T>(string Endpoint, string token) where T : class
    {
      var apiResponse = new ApiResponse<T>((int)HttpStatusCode.OK);

      HttpRequestMessage httpRequestMessage = new();

      httpRequestMessage.Method = new HttpMethod(HttpMethod.Put.ToString());

      httpRequestMessage.RequestUri = new Uri(Endpoint);


      if (!string.IsNullOrEmpty(token))
      {
        httpRequestMessage.Headers.Authorization
           = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
      }

      var result = await _http.SendAsync(httpRequestMessage);

      dynamic content = null;
      content = await result.Content.ReadAsStringAsync();



      if (result.StatusCode == HttpStatusCode.InternalServerError ||
         result.StatusCode == HttpStatusCode.BadRequest ||
         result.StatusCode == HttpStatusCode.Unauthorized)
      {
        Console.WriteLine(content);

        apiResponse.StatusCode = (int)result.StatusCode;
        return apiResponse;
      }


      try
      {

        return apiResponse;

      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message + e.StackTrace);
        Console.WriteLine(content);
        apiResponse.ErrorMessage = e.Message;

        return apiResponse;

      }
    }


  }
}
