using Akka.Actor;
using Akka.Network.Succinctly.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace akkaCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        //private ActorSystem _actorSystem;
        //private readonly ICalculatorActorInstance CalculatorActor;


        //public CalculatorController(ActorSystem actorSystem)

        //{

        //    _actorSystem = actorSystem;

        //}
        private readonly ICalculatorActorInstance CalculatorActor;

        public CalculatorController(ICalculatorActorInstance calculatorActor)

        {

            CalculatorActor = calculatorActor;

        }
        [HttpGet("sum")]

        public async Task<double> Sum(double x, double y)

        {
            //method 1 
            //var calculatorActorProps = Props.Create<CalculatorActor>();

            //var calculatorRef = _actorSystem.ActorOf(calculatorActorProps);

            //AddMessage addMessage = new AddMessage(x, y);

            //AnswerMessage answer = await calculatorRef.Ask<AnswerMessage>(addMessage);

            //return answer.Value;

            //method 2 it is better way to connect to this actor because it doesn't create an instance of this object 
            //IActorRef calculatorRef;
            //try
            //{
            //    calculatorRef = await _actorSystem.ActorSelection("/user/calculator").ResolveOne(TimeSpan.FromMilliseconds(100));
            //}
            //catch (ActorNotFoundException exc)
            //{
            //    var calculatorActorProps = Props.Create<CalculatorActor>();
            //    calculatorRef = _actorSystem.ActorOf(calculatorActorProps, "calculator");
            //}
            //AddMessage addMessage = new AddMessage(x, y);
            //AnswerMessage answer = await calculatorRef.Ask<AnswerMessage>(addMessage);
            //return answer.Value;
            //method 3

            var answer = await CalculatorActor.Sum(new AddMessage(x, y));

            return answer.Value;
        }
    }
}
