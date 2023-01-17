using AuthO.API;

var builder = WebApplication.CreateBuilder(args);


//using startup class to separate codes
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);


var app = builder.Build();


startup.Configure(app, app.Environment);

app.MapControllers();

app.Run();
