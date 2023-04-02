using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;
using eShopOnTelegram.Domain.Dto.Customers;
using eShopOnTelegram.Domain.Requests;

namespace eShopOnTelegram.Admin.Endpoints.Customers;

public class GetCustomers : EndpointBaseAsync
    .WithRequest<GetRequest>
    .WithActionResult<IEnumerable<CustomerDto>>
{
    private readonly ICustomerService _customerService;

    public GetCustomers(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("/api/customers")]
    [SwaggerOperation(Tags = new[] { SwaggerGroup.Customers})]
    public override async Task<ActionResult<IEnumerable<CustomerDto>>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _customerService.GetMultipleAsync(request, cancellationToken);

        Response.AddPaginationHeaders(request.PaginationModel, response.TotalItemsInDatabase);

        return response.AsActionResult();
    }
}
