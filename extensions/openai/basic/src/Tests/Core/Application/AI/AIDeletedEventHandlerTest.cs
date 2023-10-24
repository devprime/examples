namespace Core.Tests;
public class AIDeletedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.aievents", "aievents");
        return settings;
    }
    private AIDeletedEventDTO SetEventData(Domain.Aggregates.AI.AI ai)
    {
        return new AIDeletedEventDTO()
        {ID = ai.ID, Prompt = ai.Prompt};
    }
    public AIDeleted Create_AI_Object_OK(DpTest dpTest)
    {
        var ai = AITest.Create_AI_Required_Properties_OK(dpTest);
        var aiDeleted = new AIDeleted();
        dpTest.SetDomainEventObject(aiDeleted, ai);
        return aiDeleted;
    }
    [Fact]
    [Trait("EventHandler", "AIDeletedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_AIObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var aiDeleted = Create_AI_Object_OK(dpTest);
        var ai = dpTest.GetDomainEventObject<Domain.Aggregates.AI.AI>(aiDeleted);
        var aiDeletedEventHandler = new Application.EventHandlers.AI.AIDeletedEventHandler(null, dpTest.MockDp<IAIState>(null));
        dpTest.SetupSettings(aiDeletedEventHandler.Dp, settings);
        dpTest.SetupStream(aiDeletedEventHandler.Dp);
        //Act
        var result = aiDeletedEventHandler.Handle(aiDeleted);
        //Assert
        var sentEvents = dpTest.GetSentEvents(aiDeletedEventHandler.Dp);
        var aiDeletedEventDTO = SetEventData(ai);
        Assert.Equal(sentEvents[0].Destination, settings["stream.aievents"]);
        Assert.Equal("AIDeleted", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, aiDeletedEventDTO);
        Assert.Equal(result, true);
    }
}