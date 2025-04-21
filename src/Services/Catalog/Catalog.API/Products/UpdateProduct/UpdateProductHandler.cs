
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductCommandResult>;
    public record UpdateProductCommandResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("ID is Required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is Required")
                .Length(2, 150).WithMessage("Name Must be between 2 and 150 characters");

            RuleFor(x => x.Categories)
                .NotEmpty()
                .WithMessage("Atleast one Category is Required");

            RuleFor(x => x.ImageFile)
                .NotEmpty()
                .WithMessage("Image is Required");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be more than 0");
        }
    }

    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
    {
        private readonly IDocumentSession _session;

        public UpdateProductHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            Product product = await _session.LoadAsync<Product>(request.Id, cancellationToken) 
                ?? throw new ProductNotFoundException(request.Id);
            
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
