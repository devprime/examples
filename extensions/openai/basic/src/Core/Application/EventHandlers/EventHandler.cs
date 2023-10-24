namespace Application.EventHandlers;
public class EventHandler : IEventHandler
{
    public EventHandler(IHandler handler)
    {
        handler.Add<AICreated, AICreatedEventHandler>();
        handler.Add<AIDeleted, AIDeletedEventHandler>();
        handler.Add<AIGetByID, AIGetByIDEventHandler>();
        handler.Add<AIGet, AIGetEventHandler>();
        handler.Add<AIUpdated, AIUpdatedEventHandler>();
        handler.Add<CreateAI, CreateAIEventHandler>();
        handler.Add<DeleteAI, DeleteAIEventHandler>();
        handler.Add<UpdateAI, UpdateAIEventHandler>();
    }
}