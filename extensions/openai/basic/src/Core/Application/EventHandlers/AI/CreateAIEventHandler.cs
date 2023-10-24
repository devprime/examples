namespace Application.EventHandlers.AI;
public class CreateAIEventHandler : EventHandlerWithExtensions<CreateAI, IExtensions>
{
    public CreateAIEventHandler(IExtensions extensions, IDp dp) : base(extensions, dp)
    {
    }
    public override dynamic Handle(CreateAI createAI)
    {
        var aI = createAI.Get<Domain.Aggregates.AI.AI>();
        var result = Dp.Extensions.IntelligenceService.Conversation(aI.Prompt);
        return result;
    }
}