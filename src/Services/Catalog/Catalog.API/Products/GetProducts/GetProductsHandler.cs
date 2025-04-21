namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery() : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);

    public class GetProductsHandler : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        private readonly IDocumentSession _session;

        public GetProductsHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            IReadOnlyList<Product> products = await _session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductResult(products);
        }
    }
}
