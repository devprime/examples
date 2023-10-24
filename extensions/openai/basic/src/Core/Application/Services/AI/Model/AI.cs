namespace Application.Services.AI.Model;
public class AI
{
    internal int? Limit { get; set; }
    internal int? Offset { get; set; }
    internal string Ordering { get; set; }
    internal string Filter { get; set; }
    internal string Sort { get; set; }
    public AI(int? limit, int? offset, string ordering, string sort, string filter)
    {
        Limit = limit;
        Offset = offset;
        Ordering = ordering;
        Filter = filter;
        Sort = sort;
    }
    public Guid ID { get; set; }
    public string Prompt { get; set; }
    public virtual PagingResult<IList<AI>> ToAIList(IList<Domain.Aggregates.AI.AI> aIList, long? total, int? offSet, int? limit)
    {
        var _aIList = ToApplication(aIList);
        return new PagingResult<IList<AI>>(_aIList, total, offSet, limit);
    }
    public virtual AI ToAI(Domain.Aggregates.AI.AI aI)
    {
        var _aI = ToApplication(aI);
        return _aI;
    }
    public virtual Domain.Aggregates.AI.AI ToDomain()
    {
        var _aI = ToDomain(this);
        return _aI;
    }
    public virtual Domain.Aggregates.AI.AI ToDomain(Guid id)
    {
        var _aI = new Domain.Aggregates.AI.AI();
        _aI.ID = id;
        return _aI;
    }
    public AI()
    {
    }
    public AI(Guid id)
    {
        ID = id;
    }
    public static Application.Services.AI.Model.AI ToApplication(Domain.Aggregates.AI.AI aI)
    {
        if (aI is null)
            return new Application.Services.AI.Model.AI();
        Application.Services.AI.Model.AI _aI = new Application.Services.AI.Model.AI();
        _aI.ID = aI.ID;
        _aI.Prompt = aI.Prompt;
        return _aI;
    }
    public static List<Application.Services.AI.Model.AI> ToApplication(IList<Domain.Aggregates.AI.AI> aIList)
    {
        List<Application.Services.AI.Model.AI> _aIList = new List<Application.Services.AI.Model.AI>();
        if (aIList != null)
        {
            foreach (var aI in aIList)
            {
                Application.Services.AI.Model.AI _aI = new Application.Services.AI.Model.AI();
                _aI.ID = aI.ID;
                _aI.Prompt = aI.Prompt;
                _aIList.Add(_aI);
            }
        }
        return _aIList;
    }
    public static Domain.Aggregates.AI.AI ToDomain(Application.Services.AI.Model.AI aI)
    {
        if (aI is null)
            return new Domain.Aggregates.AI.AI();
        Domain.Aggregates.AI.AI _aI = new Domain.Aggregates.AI.AI(aI.ID, aI.Prompt);
        return _aI;
    }
    public static List<Domain.Aggregates.AI.AI> ToDomain(IList<Application.Services.AI.Model.AI> aIList)
    {
        List<Domain.Aggregates.AI.AI> _aIList = new List<Domain.Aggregates.AI.AI>();
        if (aIList != null)
        {
            foreach (var aI in aIList)
            {
                Domain.Aggregates.AI.AI _aI = new Domain.Aggregates.AI.AI(aI.ID, aI.Prompt);
                _aIList.Add(_aI);
            }
        }
        return _aIList;
    }
}