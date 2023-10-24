namespace Application.EventHandlers.AI;
public class AIGetEventHandler : EventHandler<AIGet, IAIState>
{
    public AIGetEventHandler(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public override dynamic Handle(AIGet domainEvent)
    {
        var source = Dp.State.AI.GetAll(domainEvent.Limit, domainEvent.Offset, domainEvent.Ordering, domainEvent.Sort, domainEvent.Filter);
        var total = Dp.State.AI.Total(domainEvent.Filter);
        return (source, total);
    }
}