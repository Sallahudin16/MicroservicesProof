WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});



WebApplication app = builder.Build();
app.Run();
