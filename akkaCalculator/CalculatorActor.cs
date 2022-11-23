using Akka.Actor;
using Akka.Network.Succinctly.Core.Common;

namespace akkaCalculator
{
    //public class CalculatorActor: ReceiveActor
    //{
    //    public CalculatorActor()

    //    {

    //        Receive<AddMessage>(add =>

    //        {

    //            Sender.Tell(new AnswerMessage(add.Term1 + add.Term2));

    //        });

    //    }
    //}
    //public class AddMessage

    //{

    //    public AddMessage(double term1, double term2)

    //    {

    //        Term1 = term1;

    //        Term2 = term2;

    //    }

    //    public double Term1;

    //    public double Term2;

    //}
    //public class AnswerMessage

    //{

    //    public AnswerMessage(double value)

    //    {

    //        Value = value;

    //    }

    //    public double Value;

    //}
    public interface ICalculatorActorInstance

    {

        Task<AnswerMessage> Sum(AddMessage message);

    }

    public class CalculatorActorInstance : ICalculatorActorInstance

    {

        private IActorRef _actor;

        public CalculatorActorInstance(ActorSystem actorSystem)

        {

            _actor = actorSystem.ActorOf(Props.Create<CalculatorActor>(), "calculator");

        }

        public async Task<AnswerMessage> Sum(AddMessage message)

        {

            return await _actor.Ask<AnswerMessage>(message);

        }

    }

}
