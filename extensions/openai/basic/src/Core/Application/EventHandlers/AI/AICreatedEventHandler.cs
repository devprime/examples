namespace Application.EventHandlers.AI;
public class AICreatedEventHandler : EventHandler<AICreated, IAIState>
{
    public AICreatedEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(AICreated aICreated)
    {
        var success = false;
        var aI = aICreated.Get<Domain.Aggregates.AI.AI>();
        var destination = Dp.Settings.Default("stream.aievents");
        var eventName = "AICreated";
        var eventData = new AICreatedEventDTO()
        {ID = aI.ID, Prompt = aI.Prompt};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}