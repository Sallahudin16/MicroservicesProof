
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductCommandResult>;
    public record UpdateProductCommandResult(bool IsSuccess);

    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product with id {Id}", request.Id);

            Product product = await _session.LoadAsync<Product>(request.Id, cancellationToken) 
                ?? throw new ProductNotFoundException();
            
            product.Name = request.Name;
            product.Categories = request.Categories;
            product.Description = request.Description;
            product.ImageFile = request.ImageFile;
            product.Price = request.Price;

            _session.Update(product);
            await _session.SaveChangesAsync(cancellationToken);

            return new UpdateProductCommandResult(true);
        }
    }
}
