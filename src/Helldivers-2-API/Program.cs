using Helldivers.API.Controllers;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddProblemDetails();

#if DEBUG
// Swagger is generated at compile time, so we don't include Swagger dependencies in Release builds.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endif

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/war-season", WarSeasonController.Current);

app.Run();
