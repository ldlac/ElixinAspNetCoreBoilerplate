using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace ElixinBackend.Utils
{
    public static class Endpoints
    {
        public static async Task Created(this HttpResponse response, object payload)
        {
            await BaseResponse(response, HttpStatusCode.Created, payload);
        }

        public static async Task Ok(this HttpResponse response, object payload)
        {
            await BaseResponse(response, HttpStatusCode.OK, payload);
        }

        public static async Task BadRequest(this HttpResponse response, object payload)
        {
            await BaseResponse(response, HttpStatusCode.BadRequest, payload);
        }

        public static async Task NotFound(this HttpResponse response, object payload)
        {
            await BaseResponse(response, HttpStatusCode.NotFound, payload);
        }

        private static async Task BaseResponse(HttpResponse response, HttpStatusCode httpStatusCode, object payload)
        {
            response.StatusCode = (int)httpStatusCode;

            await response.WriteJson(payload);
        }
    }
}