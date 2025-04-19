namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        private const string _moduleRoute = "/products";
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(_moduleRoute, async (CreateProductRequest request, ISender sender) =>
            {
                CreateProductCommand command = request.Adapt<CreateProductCommand>();
                CreateProductResult result = await sender.Send(command);
                CreateProductResponse response = result.Adapt<CreateProductResponse>();
                return Results.Created($"{_moduleRoute}/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product Endpoint");
        }
    }
}
