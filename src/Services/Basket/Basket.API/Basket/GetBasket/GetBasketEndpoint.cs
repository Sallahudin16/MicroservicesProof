
namespace Basket.API.Basket.GetBasket
{
    //public record GetBasketRequest(string UserName);
    public record GetBasketResponse(ShoppingCart Cart);

    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                GetBasketResult result = await sender.Send(new GetBasketQuery(userName));
                return Results.Ok( result.Adapt<GetBasketResponse>() );
            })
                .WithName("GetUserBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK);
        }
    }
}
