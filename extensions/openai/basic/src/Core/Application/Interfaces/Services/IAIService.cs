namespace Application.Interfaces.Services;
public interface IAIService
{
    string Add(Application.Services.AI.Model.AI command);
    void Update(Application.Services.AI.Model.AI command);
    void Delete(Application.Services.AI.Model.AI command);
    Application.Services.AI.Model.AI Get(Application.Services.AI.Model.AI query);
    PagingResult<IList<Application.Services.AI.Model.AI>> GetAll(Application.Services.AI.Model.AI query);
}