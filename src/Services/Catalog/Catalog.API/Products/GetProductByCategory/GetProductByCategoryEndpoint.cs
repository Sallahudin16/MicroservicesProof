
namespace Catalog.API.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest();
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(RoutingConstants.ProductsRoute + "/category/{categoryName}", async (string categoryName, ISender sender) =>
            {
                GetProductByCategoryResult result = await sender.Send(new GetProductByCategoryQuery(categoryName));
                return Results.Ok( result.Adapt<GetProductByCategoryResponse>() );
            })
                .WithName("GetProductByCategory")
                .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(RoutingConstants.ModuleName);
        }
    }
}
