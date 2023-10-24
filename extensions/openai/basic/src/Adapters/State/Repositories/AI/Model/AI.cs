namespace DevPrime.State.Repositories.AI.Model;
public class AI
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId _Id { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid ID { get; set; }
    public string Prompt { get; set; }
}