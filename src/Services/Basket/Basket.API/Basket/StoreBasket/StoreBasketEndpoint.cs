using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async ([FromBody] StoreBasketRequest request, ISender sender) =>
            {
                StoreBasketResult result = await sender.Send( request.Adapt<StoreBasketCommand>() );
                return Results.Ok(result.Adapt<StoreBasketResponse>());
            })
                .WithName("StoreUserBasket")
                .WithSummary("Store User Basket")
                .WithDescription("Stores the user's shopping cart in the database and updates the cache.")
                .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
