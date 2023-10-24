namespace Application.EventHandlers.AI;
public class AIGetByIDEventHandler : EventHandler<AIGetByID, IAIState>
{
    public AIGetByIDEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(AIGetByID aIGetByID)
    {
        var aI = aIGetByID.Get<Domain.Aggregates.AI.AI>();
        var result = Dp.State.AI.Get(aI.ID);
        return result;
    }
}