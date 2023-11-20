using eShopOnTelegram.Domain.Responses;

namespace eShopOnTelegram.Shop.Api.Extensions;

public static class ResponseExtensions
{
    public static ActionResult AsActionResult(this Response response)
    {
        if (response.Status == ResponseStatus.ValidationFailed)
        {
            return new BadRequestResult();
        }

        if (response.Status == ResponseStatus.NotFound)
        {
            return new NotFoundResult();
        }

        if (response.Status == ResponseStatus.Exception)
        {
            return new StatusCodeResult(503);
        }

        return new OkObjectResult(response);
    }

    public static ActionResult AsActionResult<T>(this Response<T> response) where T : class
    {
        if (response.Status == ResponseStatus.ValidationFailed)
        {
            return new BadRequestResult();
        }

        if (response.Status == ResponseStatus.NotFound)
        {
            return new NotFoundResult();
        }

        if (response.Status == ResponseStatus.Exception)
        {
            return new StatusCodeResult(503);
        }

        return new OkObjectResult(response.Data);
    }
}
