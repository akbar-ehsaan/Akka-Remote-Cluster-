// See https://aka.ms/new-console-template for more information
using Akka.Actor;
using Akka.Cluster;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using Akka.Event;
using System.Configuration;

StartUp(args.Length == 0 ? new String[] { "2551", "2552", "0" } : args);
Console.WriteLine("Press any key to exit");
Console.ReadLine();


static void StartUp(string[] ports)
{
    var hocon = File.ReadAllText("hocon.conf");
    var conf = ConfigurationFactory.ParseString(hocon);

    foreach (var port in ports)
    {
        //Override the configuration of the port
        var config =
            ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
                .WithFallback(conf);

        //create an Akka system
        var system = ActorSystem.Create("ClusterSystem", config);

        //create an actor that handles cluster domain events
        system.ActorOf(Props.Create(typeof(SimpleClusterListener)), "clusterListener");
    }

}
public class SimpleClusterListener : UntypedActor
{
    protected ILoggingAdapter Log = Context.GetLogger();
    protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);

    /// <summary>
    /// Need to subscribe to cluster changes
    /// </summary>
    protected override void PreStart()
    {
        Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, new[] { typeof(ClusterEvent.IMemberEvent), typeof(ClusterEvent.UnreachableMember) });
    }

    /// <summary>
    /// Re-subscribe on restart
    /// </summary>
    protected override void PostStop()
    {
        Cluster.Unsubscribe(Self);
    }

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case ClusterEvent.MemberUp up:
                {
                    var mem = up;
                    Log.Info("Member is Up: {0}", mem.Member);
                    break;
                }
            case ClusterEvent.MemberJoined joined:
                Log.Info("Member is joining {0}", joined.Member);
                break;
            case ClusterEvent.UnreachableMember unreachable:
                Log.Info("Member detected as unreachable: {0}", unreachable.Member);
                break;
            case ClusterEvent.MemberRemoved removed:
                Log.Info("Member is Removed: {0}", removed.Member);
                break;
            case ClusterEvent.IMemberEvent _:
                //IGNORE                
                break;
            case ClusterEvent.CurrentClusterState _:
                break;
            default:
                Unhandled(message);
                break;
        }
    }
}