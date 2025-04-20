
namespace Catalog.API.Products.GetProducts
{
    //public record GetProductsRequest();
    public record GetProductResponse(IEnumerable<Product> Products);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(RoutingConstants.ProductsRoute, async (ISender sender) =>
            {
                GetProductsQuery query = new();
                GetProductResult result = await sender.Send(query);
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
