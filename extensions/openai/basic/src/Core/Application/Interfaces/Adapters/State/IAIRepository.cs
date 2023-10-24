namespace Application.Interfaces.Adapters.State;
public interface IAIRepository
{
    bool Add(Domain.Aggregates.AI.AI source);
    bool Delete(Guid Id);
    bool Update(Domain.Aggregates.AI.AI source);
    Domain.Aggregates.AI.AI Get(Guid Id);
    List<Domain.Aggregates.AI.AI> GetAll(int? limit, int? offset, string ordering, string sort, string filter);
    bool Exists(Guid id);
    long Total(string filter);
}