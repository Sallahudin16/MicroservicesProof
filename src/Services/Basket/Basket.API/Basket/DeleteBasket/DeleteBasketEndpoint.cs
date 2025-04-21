namespace Basket.API.Basket.DeleteBasket
{
    //public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool Success);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                DeleteBasketResult result = await sender.Send(new DeleteBasketCommand(userName));
                return Results.Ok(result.Adapt<DeleteBasketResponse>());
            })
                .WithName("DeleteUserBasket")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithSummary("Delete User Basket")
                .WithDescription("Deletes the user's shopping cart from the database and cache.");
        }
    }
}
