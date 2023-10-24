namespace DevPrime.State.Repositories.AI;
public class AIRepository : RepositoryBase, IAIRepository
{
    public AIRepository(IDpState dp) : base(dp)
    {
        ConnectionAlias = "State1";
    }

#region Write
    public bool Add(Domain.Aggregates.AI.AI aI)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _aI = ToState(aI);
            state.AI.InsertOne(_aI);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }
    public bool Delete(Guid aIID)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            state.AI.DeleteOne(p => p.ID == aIID);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }
    public bool Update(Domain.Aggregates.AI.AI aI)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var _aI = ToState(aI);
            _aI._Id = state.AI.Find(p => p.ID == aI.ID).FirstOrDefault()._Id;
            state.AI.ReplaceOne(p => p.ID == aI.ID, _aI);
            return true;
        });
        if (result is null)
            return false;
        return result;
    }

#endregion Write

#region Read
    public Domain.Aggregates.AI.AI Get(Guid aIID)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var aI = state.AI.Find(p => p.ID == aIID).FirstOrDefault();
            var _aI = ToDomain(aI);
            return _aI;
        });
    }
    public List<Domain.Aggregates.AI.AI> GetAll(int? limit, int? offset, string ordering, string sort, string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            List<Model.AI> aI = null;
            if (sort?.ToLower() == "desc")
            {
                var result = state.AI.Find(GetFilter(filter)).SortByDescending(GetOrdering(ordering));
                if (limit != null && offset != null)
                    aI = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    aI = result.ToList();
            }
            else if (sort?.ToLower() == "asc")
            {
                var result = state.AI.Find(GetFilter(filter)).SortBy(GetOrdering(ordering));
                if (limit != null && offset != null)
                    aI = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    aI = result.ToList();
            }
            else
            {
                var result = state.AI.Find(GetFilter(filter));
                if (limit != null && offset != null)
                    aI = result.Skip((offset - 1) * limit).Limit(limit).ToList();
                else
                    aI = result.ToList();
            }
            var _aI = ToDomain(aI);
            return _aI;
        });
    }
    private Expression<Func<Model.AI, object>> GetOrdering(string field)
    {
        Expression<Func<Model.AI, object>> exp = p => p.ID;
        if (!string.IsNullOrWhiteSpace(field))
        {
            if (field.ToLower() == "prompt")
                exp = p => p.Prompt;
            else
                exp = p => p.ID;
        }
        return exp;
    }
    private FilterDefinition<Model.AI> GetFilter(string filter)
    {
        var builder = Builders<Model.AI>.Filter;
        FilterDefinition<Model.AI> exp;
        string Prompt = string.Empty;
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var conditions = filter.Split(",");
            if (conditions.Count() >= 1)
            {
                foreach (var condition in conditions)
                {
                    var slice = condition?.Split("=");
                    if (slice.Length > 1)
                    {
                        var field = slice[0];
                        var value = slice[1];
                        if (field.ToLower() == "prompt")
                            Prompt = value;
                    }
                }
            }
        }
        var bfilter = builder.Empty;
        if (!string.IsNullOrWhiteSpace(Prompt))
        {
            var PromptFilter = builder.Eq(x => x.Prompt, Prompt);
            bfilter &= PromptFilter;
        }
        exp = bfilter;
        return exp;
    }
    public bool Exists(Guid aIID)
    {
        var result = Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var aI = state.AI.Find(x => x.ID == aIID).Project<Model.AI>("{ ID: 1 }").FirstOrDefault();
            return (aIID == aI?.ID);
        });
        if (result is null)
            return false;
        return result;
    }
    public long Total(string filter)
    {
        return Dp.Pipeline(ExecuteResult: (stateContext) =>
        {
            var state = new ConnectionMongo(stateContext, Dp);
            var total = state.AI.Find(GetFilter(filter)).CountDocuments();
            return total;
        });
    }

#endregion Read

#region mappers
    public static DevPrime.State.Repositories.AI.Model.AI ToState(Domain.Aggregates.AI.AI aI)
    {
        if (aI is null)
            return new DevPrime.State.Repositories.AI.Model.AI();
        DevPrime.State.Repositories.AI.Model.AI _aI = new DevPrime.State.Repositories.AI.Model.AI();
        _aI.ID = aI.ID;
        _aI.Prompt = aI.Prompt;
        return _aI;
    }
    public static Domain.Aggregates.AI.AI ToDomain(DevPrime.State.Repositories.AI.Model.AI aI)
    {
        if (aI is null)
            return new Domain.Aggregates.AI.AI()
            {IsNew = true};
        Domain.Aggregates.AI.AI _aI = new Domain.Aggregates.AI.AI(aI.ID, aI.Prompt);
        return _aI;
    }
    public static List<Domain.Aggregates.AI.AI> ToDomain(IList<DevPrime.State.Repositories.AI.Model.AI> aIList)
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

#endregion mappers
}