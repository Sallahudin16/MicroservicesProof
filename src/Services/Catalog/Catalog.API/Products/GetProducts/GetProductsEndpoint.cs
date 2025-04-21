
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int PageNumber = 1, int PageSize = 10);
    public record GetProductResponse(IEnumerable<Product> Products);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(RoutingConstants.ProductsRoute, async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                GetProductResult result = await sender.Send( request.Adapt<GetProductsQuery>() );
                GetProductResponse response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);
            })
                .WithName("GetProducts")
                .Produces<GetProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDescription("Gets all Products");
        }
    }
}
