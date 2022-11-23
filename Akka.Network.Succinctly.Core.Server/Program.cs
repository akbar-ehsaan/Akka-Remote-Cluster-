// See https://aka.ms/new-console-template for more information
using Akka.Actor;
using Akka.Network.Succinctly.Core.Common;

Console.WriteLine("Hello, World!");
var hocon = HoconLoader.FromFile("akka.net.hocon.conf");

ActorSystem system = ActorSystem.Create("server-system", hocon);

Console.WriteLine("Server started");

Console.Read();

system.Terminate().Wait();
