namespace Domain.Aggregates.AI;
public class AI : AggRoot
{
    public string Prompt { get; private set; }
    public AI(Guid id, string prompt)
    {
        ID = id;
        Prompt = prompt;
    }
    public AI()
    {
    }
    public virtual string Add()
    {

        var result = Dp.Pipeline(ExecuteResult: () =>
        {
            ValidFields();
            ID = Guid.NewGuid();
            IsNew = true;
            var processresult = Dp.ProcessEvent<string>(new CreateAI());
            return processresult;
        });
        return result;
    }
    public virtual void Update()
    {
        Dp.Pipeline(Execute: () =>
        {
            if (ID.Equals(Guid.Empty))
                Dp.Notifications.Add("ID is required");
            ValidFields();
            var success = Dp.ProcessEvent<bool>(new UpdateAI());
            if (success)
            {
                Dp.ProcessEvent(new AIUpdated());
            }
        });
    }
    public virtual void Delete()
    {
        Dp.Pipeline(Execute: () =>
        {
            if (ID != Guid.Empty)
            {
                var success = Dp.ProcessEvent<bool>(new DeleteAI());
                if (success)
                {
                    Dp.ProcessEvent(new AIDeleted());
                }
            }
        });
    }
    public virtual (List<AI> Result, long Total) Get(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            ValidateOrdering(limit, offset, ordering, sort);
            if (!string.IsNullOrWhiteSpace(filter))
            {
                bool filterIsValid = false;
                if (filter.Contains("="))
                {
                    if (filter.ToLower().StartsWith("id="))
                        filterIsValid = true;
                    if (filter.ToLower().StartsWith("prompt="))
                        filterIsValid = true;
                }
                if (!filterIsValid)
                    throw new PublicException($"Invalid filter '{filter}' is invalid try: 'ID', 'Prompt',");
            }
            var source = Dp.ProcessEvent(new AIGet()
            {Limit = limit, Offset = offset, Ordering = ordering, Sort = sort, Filter = filter});
            return source;
        });
    }
    public virtual AI GetByID()
    {
        var result = Dp.Pipeline(ExecuteResult: () =>
        {
            return Dp.ProcessEvent<AI>(new AIGetByID());
        });
        return result;
    }
    private void ValidFields()
    {
        if (String.IsNullOrWhiteSpace(Prompt))
            Dp.Notifications.Add("Prompt is required");
        Dp.Notifications.ValidateAndThrow();
    }
}