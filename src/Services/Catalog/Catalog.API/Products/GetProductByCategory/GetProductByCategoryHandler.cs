namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductByCategoryHandler : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        private readonly IDocumentSession _session;
        private readonly ILogger<GetProductByCategoryHandler> _logger;

        public GetProductByCategoryHandler(IDocumentSession session, ILogger<GetProductByCategoryHandler> logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting products with category {Category}", request.Category);

            IReadOnlyList<Product> products = await _session.Query<Product>()
                .Where(x => x.Categories.Contains(request.Category))
                .ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(products);
        }
    }
}
