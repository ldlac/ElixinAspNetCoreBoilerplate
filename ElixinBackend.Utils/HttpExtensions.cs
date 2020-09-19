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
        public static IEndpointConventionBuilder MapPost<T>(this IEndpointRouteBuilder endpoints, string pattern, Func<IServiceProvider, T, HttpResponse, Task> requestDelegate)
        {
            return endpoints.MapPost(pattern, async context =>
            {
                var body = await context.ReadFromJson<T>();

                await requestDelegate(context.RequestServices, body, context.Response);
            });
        }

        public static Task WriteJson<T>(this HttpResponse response, T @object)
        {
            response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(@object, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return response.WriteAsync(json);
        }

        public static async Task<T> ReadFromJson<T>(this HttpContext httpContext)
        {
            using StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);

            var @object = JsonConvert.DeserializeObject<T>(await reader.ReadToEndAsync());

            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(@object, new ValidationContext(@object), results))
            {
                return @object;
            }

            httpContext.Response.StatusCode = 400;

            await httpContext.Response.WriteJson(results);

            return default;
        }
    }
}