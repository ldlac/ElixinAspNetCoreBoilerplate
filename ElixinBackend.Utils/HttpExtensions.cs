using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace ElixinBackend.Utils
{
    public static class HttpExtensions
    {
        public static void MapPost<T>(this IEndpointRouteBuilder endpoints, string pattern, Func<IServiceProvider, T, HttpResponse, Task> c)
        {
            endpoints.MapPost(pattern, async context =>
            {
                var body = await context.ReadFromJson<T>();

                await c(context.RequestServices, body, context.Response);
            });
        }

        public static Task WriteJson<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return response.WriteAsync(json);
        }

        public static async Task<T> ReadFromJson<T>(this HttpContext httpContext)
        {
            using StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);

            var t = await reader.ReadToEndAsync();

            var obj = JsonConvert.DeserializeObject<T>(t);

            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(obj, new ValidationContext(obj), results))
            {
                return obj;
            }

            httpContext.Response.StatusCode = 400;

            await httpContext.Response.WriteJson(results);

            return default;
        }
    }
}