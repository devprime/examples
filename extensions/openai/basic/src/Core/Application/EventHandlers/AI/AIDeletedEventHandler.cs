namespace Application.EventHandlers.AI;
public class AIDeletedEventHandler : EventHandler<AIDeleted, IAIState>
{
    public AIDeletedEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(AIDeleted aIDeleted)
    {
        var success = false;
        var aI = aIDeleted.Get<Domain.Aggregates.AI.AI>();
        var destination = Dp.Settings.Default("stream.aievents");
        var eventName = "AIDeleted";
        var eventData = new AIDeletedEventDTO()
        {ID = aI.ID, Prompt = aI.Prompt};
        Dp.Stream.Send(destination, eventName, eventData);
        success = true;
        return success;
    }
}