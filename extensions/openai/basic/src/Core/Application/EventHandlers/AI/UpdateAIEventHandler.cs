namespace Application.EventHandlers.AI;
public class UpdateAIEventHandler : EventHandler<UpdateAI, IAIState>
{
    public UpdateAIEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(UpdateAI updateAI)
    {
        var aI = updateAI.Get<Domain.Aggregates.AI.AI>();
        var result = Dp.State.AI.Update(aI);
        return result;
    }
}