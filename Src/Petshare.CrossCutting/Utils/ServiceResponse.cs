using System.Net;

namespace Petshare.CrossCutting.Utils;

public class ServiceResponse
{
    public HttpStatusCode StatusCode { get; }

    public object? Data { get; }

    private ServiceResponse(HttpStatusCode statusCode, object? data = null)
    {
        StatusCode = statusCode;
        Data = data;
    }

    public static ServiceResponse Ok(object? data = null) => new(HttpStatusCode.OK, data);

    public static ServiceResponse Created(object? data = null) => new(HttpStatusCode.Created, data);

    public static ServiceResponse NotFound() => new(HttpStatusCode.NotFound);

    public static ServiceResponse BadRequest() => new(HttpStatusCode.BadRequest);
}