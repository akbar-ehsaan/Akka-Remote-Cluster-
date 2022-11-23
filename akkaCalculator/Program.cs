using Akka.Actor;
using Akka.Network.Succinctly.Core.Common;
using akkaCalculator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var actorSystem = ActorSystem.Create("calculator-actor-system");
//builder.Services.AddSingleton(typeof(ActorSystem), (serviceProvider) => actorSystem);


builder.Services.AddControllers();

//var actorSystem = ActorSystem.Create("calculator-actor-system");
//builder.Services.AddSingleton(typeof(ActorSystem), (serviceProvider) => actorSystem);
//builder.Services.AddSingleton(typeof(ICalculatorActorInstance),typeof(CalculatorActorInstance));


var hocon = HoconLoader.FromFile("akka.net.hocon.conf");

var actorSystem = ActorSystem.Create("calculator-actor-system", hocon);



builder.Services.AddSingleton(typeof(ActorSystem), (serviceProvider) => actorSystem);

builder.Services.AddSingleton(typeof(ICalculatorActorInstance), typeof(CalculatorActorInstance));

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
