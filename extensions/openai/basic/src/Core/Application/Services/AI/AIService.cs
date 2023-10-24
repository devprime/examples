
namespace Application.Services.AI;
public class AIService : ApplicationService<IAIState>, IAIService
{
    public AIService(IAIState state, IDp dp) : base(state, dp)
    {
    }
    public string Add(Model.AI command)
    {
      return Dp.Pipeline(ExecuteResult: () =>
        {
            var process = command.ToDomain();
            Dp.Attach(process);
            var processresult = process.Add();
            return processresult;
        });
    }



    public void Update(Model.AI command)
    {
        Dp.Pipeline(Execute: () =>
        {
            var aI = command.ToDomain();
            Dp.Attach(aI);
            aI.Update();
        });
    }
    public void Delete(Model.AI command)
    {
        Dp.Pipeline(Execute: () =>
        {
            var aI = command.ToDomain();
            Dp.Attach(aI);
            aI.Delete();
        });
    }
    public PagingResult<IList<Model.AI>> GetAll(Model.AI query)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            var aI = query.ToDomain();
            Dp.Attach(aI);
            var aIList = aI.Get(query.Limit, query.Offset, query.Ordering, query.Sort, query.Filter);
            var result = query.ToAIList(aIList.Result, aIList.Total, query.Offset, query.Limit);
            return result;
        });
    }
    public Model.AI Get(Model.AI query)
    {
        return Dp.Pipeline(ExecuteResult: () =>
        {
            var aI = query.ToDomain();
            Dp.Attach(aI);
            aI = aI.GetByID();
            var result = query.ToAI(aI);
            return result;
        });
    }


}