using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Requests;
using eShopOnTelegram.Domain.Responses.Customers;

namespace eShopOnTelegram.Admin.Endpoints.Customers;

public class GetCustomers : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<GetCustomersResponse>>
{
    private readonly ICustomerService _customerService;

    public GetCustomers(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("/api/customers")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Customers})]
    public override async Task<ActionResult<IEnumerable<GetCustomersResponse>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _customerService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}
