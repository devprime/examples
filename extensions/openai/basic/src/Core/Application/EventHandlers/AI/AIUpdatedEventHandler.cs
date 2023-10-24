namespace Application.EventHandlers.AI;
public class AIUpdatedEventHandler : EventHandler<AIUpdated, IAIState>
{
    public AIUpdatedEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(AIUpdated aIUpdated)
    {
        var success = false;
        var aI = aIUpdated.Get<Domain.Aggregates.AI.AI>();
        var destination = Dp.Settings.Default("stream.aievents");
        var eventName = "AIUpdated";
        var eventData = new AIUpdatedEventDTO()
        {ID = aI.ID, Prompt = aI.Prompt};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}