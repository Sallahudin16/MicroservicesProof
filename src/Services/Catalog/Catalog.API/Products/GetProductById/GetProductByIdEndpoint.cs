namespace Catalog.API.Products.GetProductById
{

    //public record GetProductByIdRequest();
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(RoutingConstants.ProductsRoute + "/{id:guid}", async (Guid id, ISender sender) =>
            {
                GetProductByIdResult result = await sender.Send(new GetProductByIdQuery(id));
                return Results.Ok(result.Adapt<GetProductByIdResponse>());
            })
                .WithName("GetProductById")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags([RoutingConstants.ModuleName]);
        }
    }
}
