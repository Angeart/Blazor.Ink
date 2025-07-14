using Blazor.Ink;
using Blazor.Ink.Sample;

var builder = Ink.CreateBuilder(args);
builder.UseDefaultServices();
var app = builder.Build();

await app.RunAsync<HelloInk>();