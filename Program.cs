using aspnetserver.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "https://shitpost-seven.vercel.app");
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1",new OpenApiInfo {Title = "ASP.NET React", Version ="v1"});
});  

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.DocumentTitle = "ASP.NET React";
    swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API wa ra gud");
    swaggerUIOptions.RoutePrefix = string.Empty;    
});

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync())
    .WithTags("Post Endpoints");
app.MapGet("/get-post-by-id/{postId}", async (int postId) =>
{
    Post postToReturn = await PostsRepository.GetPostByIdAsync(postId);
    return postToReturn != null ? Results.Ok(postToReturn) : Results.NotFound();
})
    .WithTags("Post Endpoints");
app.MapPost("/create-post", async (Post postToCreate) =>
 {
     bool createSuccessful = await PostsRepository.CreatePostAsync(postToCreate);
     return createSuccessful ? Results.Ok(createSuccessful) : Results.BadRequest();
 })
    .WithTags("Post Endpoints");
app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool updateSuccessful = await PostsRepository.UpdatePostAsync(postToUpdate);
    return updateSuccessful ? Results.Ok(updateSuccessful) : Results.BadRequest();
})
    .WithTags("Post Endpoints");
app.MapPost("/delete-post", async (int postToDelete) =>
{
    bool deleteSuccessful = await PostsRepository.DeletePostAsync(postToDelete);
    return deleteSuccessful ? Results.Ok(deleteSuccessful) : Results.BadRequest();
})
    .WithTags("Post Endpoints");

app.Run();

