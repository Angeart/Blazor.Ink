using Blazor.Ink;
using Blazor.Ink.Sample;

var builder = Ink.CreateBuilder(args);
builder.UseDefaultServices();
var app = builder.Build();

app.RegisterComponents([typeof(HelloInk)]);
await app.RunAsync<HelloInk>();