namespace DevPrime.State.Connections;
public class ConnectionMongo : MongoBaseState
{
    public ConnectionMongo(MongoBaseState stateContext, IDpState dp) : base(stateContext, dp)
    {
    }
    public IMongoCollection<DevPrime.State.Repositories.AI.Model.AI> AI
    {
        get
        {
            return Db.GetCollection<DevPrime.State.Repositories.AI.Model.AI>("AI");
        }
    }
}