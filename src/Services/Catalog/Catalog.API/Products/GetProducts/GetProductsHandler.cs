namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);

    public class GetProductsHandler : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<GetProductsHandler> _logger;

        public GetProductsHandler(IDocumentSession session, ILogger<GetProductsHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductsQuery with: {@Query}", query);
            IReadOnlyList<Product> products = await _session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductResult(products);
        }
    }
}
