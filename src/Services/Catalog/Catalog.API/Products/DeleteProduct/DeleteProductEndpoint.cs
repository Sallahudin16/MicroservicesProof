namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id)
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(RoutingConstants.ProductsRoute + "/{id:guid}", DeleteProductAsync)
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(RoutingConstants.ModuleName);
        }

        private async Task<IResult> DeleteProductAsync(Guid id, ISender sender)
        {
            DeleteProductResult result = await sender.Send(new DeleteProductCommand(id));
            return Results.Ok(result.Adapt<DeleteProductResponse>());
        }
    }
}
