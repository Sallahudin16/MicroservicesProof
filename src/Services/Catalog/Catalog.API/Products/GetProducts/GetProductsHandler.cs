using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<GetProductResult>;
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
            IPagedList<Product> products = await _session.Query<Product>()
                .ToPagedListAsync(query.PageNumber, query.PageSize, token: cancellationToken);
            return new GetProductResult(products);
        }
    }
}
