namespace DevPrime.Extensions;
public class Extensions : IExtensions
{
    public Extensions(IIntelligenceService intelligenceService)
    {
        IntelligenceService = intelligenceService;
    }
    public IIntelligenceService IntelligenceService { get; }
}