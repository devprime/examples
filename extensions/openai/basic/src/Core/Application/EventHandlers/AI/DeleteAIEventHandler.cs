namespace Application.EventHandlers.AI;
public class DeleteAIEventHandler : EventHandler<DeleteAI, IAIState>
{
    public DeleteAIEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(DeleteAI deleteAI)
    {
        var aI = deleteAI.Get<Domain.Aggregates.AI.AI>();
        var result = Dp.State.AI.Delete(aI.ID);
        return result;
    }
}