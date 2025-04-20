namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<GetProductByIdHandler> _logger;

        public GetProductByIdHandler(IDocumentSession session, ILogger<GetProductByIdHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting product with id {Id}", request.Id);
            Product? product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

            return product is null 
                ? throw new ProductNotFoundException(request.Id) 
                : new GetProductByIdResult(product);
        }
    }
}
