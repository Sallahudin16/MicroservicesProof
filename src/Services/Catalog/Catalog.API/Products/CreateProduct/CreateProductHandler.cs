namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is Required");
            
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

    internal class CreateProductHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //ccreate product entity
            Product product = new ()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Categories = request.Categories,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price
            };

            //save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateProductResult(product.Id);
        }
    }
}

