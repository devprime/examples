namespace DevPrime.State.States;
public class AIState : IAIState
{
    public IAIRepository AI { get; set; }
    public AIState(IAIRepository aI)
    {
        AI = aI;
    }
}