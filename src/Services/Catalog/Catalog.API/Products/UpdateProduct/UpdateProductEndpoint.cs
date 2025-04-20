
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut(RoutingConstants.ProductsRoute, UpdateProductAsync)
                .Accepts<UpdateProductRequest>("application/json")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("UpdateProduct")
                .WithTags(RoutingConstants.ModuleName);
        }

        public async Task<IResult> UpdateProductAsync([FromBody] UpdateProductRequest request, ISender sender)
        {
            UpdateProductCommandResult result = await sender.Send(request.Adapt<UpdateProductCommand>());
            return Results.Ok(result.Adapt<UpdateProductResponse>());
        }
    }
}
