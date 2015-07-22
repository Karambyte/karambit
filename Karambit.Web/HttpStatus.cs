using System;

namespace Karambit.Web
{
    public enum HttpStatus : int
    {
        OK = 200,
        Created = 201,
        NoContent = 204,
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        NotModified = 304,
        TemporaryRedirect = 307,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Gone = 410,
        InternalServerError = 500,
        NotImplemented = 501,
        ServiceUnavailable = 503,
        PermissionDenied = 550
    }
}
