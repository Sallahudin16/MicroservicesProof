using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddCarter();
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();


WebApplication app = builder.Build();
app.MapCarter();
app.Run();
