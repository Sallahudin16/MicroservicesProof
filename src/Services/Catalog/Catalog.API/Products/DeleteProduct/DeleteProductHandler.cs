namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    internal class DeleteProductHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting product with id {Id}", request.Id);

            _session.Delete<Product>(request.Id);
            await _session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
