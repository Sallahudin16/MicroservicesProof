namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly IDocumentSession _session;

        public GetProductByIdHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _session.LoadAsync<Product>(request.Id, cancellationToken);

            return product is null 
                ? throw new ProductNotFoundException(request.Id) 
                : new GetProductByIdResult(product);
        }
    }
}
