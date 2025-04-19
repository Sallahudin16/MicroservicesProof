using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

WebApplication app = builder.Build();

app.MapGet("/todoitems", async (TodoDb db) => await db.TodoItems.ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
{
    return await db.TodoItems.FindAsync(id)
        is TodoItem item
            ? Results.Ok(item)
            : Results.NotFound();
});

app.MapPost("/todoitems", async (TodoItem item, TodoDb db) =>
{
    db.TodoItems.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});

app.MapPut("/todoitems/{id}", async (int id, TodoItem inputItem, TodoDb db) =>
{
    var item = await db.TodoItems.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Name = inputItem.Name;
    item.IsComplete = inputItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.TodoItems.FindAsync(id) is TodoItem item)
    {
        db.TodoItems.Remove(item);
        await db.SaveChangesAsync();
        return Results.Ok(item);
    }
    return Results.NotFound();
});



app.Run();
