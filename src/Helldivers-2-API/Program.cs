using Helldivers.API.Controllers;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/war-season", WarSeasonController.Current);

app.Run();
