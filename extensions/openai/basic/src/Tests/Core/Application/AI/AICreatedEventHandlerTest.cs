namespace Core.Tests;
public class AICreatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.aievents", "aievents");
        return settings;
    }
    private AICreatedEventDTO SetEventData(Domain.Aggregates.AI.AI ai)
    {
        return new AICreatedEventDTO()
        {ID = ai.ID, Prompt = ai.Prompt};
    }
    public AICreated Create_AI_Object_OK(DpTest dpTest)
    {
        var ai = AITest.Create_AI_Required_Properties_OK(dpTest);
        var aiCreated = new AICreated();
        dpTest.SetDomainEventObject(aiCreated, ai);
        return aiCreated;
    }
    [Fact]
    [Trait("EventHandler", "AICreatedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_AIObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var aiCreated = Create_AI_Object_OK(dpTest);
        var ai = dpTest.GetDomainEventObject<Domain.Aggregates.AI.AI>(aiCreated);
        var aiCreatedEventHandler = new Application.EventHandlers.AI.AICreatedEventHandler(null, dpTest.MockDp<IAIState>(null));
        dpTest.SetupSettings(aiCreatedEventHandler.Dp, settings);
        dpTest.SetupStream(aiCreatedEventHandler.Dp);
        //Act
        var result = aiCreatedEventHandler.Handle(aiCreated);
        //Assert
        var sentEvents = dpTest.GetSentEvents(aiCreatedEventHandler.Dp);
        var aiCreatedEventDTO = SetEventData(ai);
        Assert.Equal(sentEvents[0].Destination, settings["stream.aievents"]);
        Assert.Equal("AICreated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, aiCreatedEventDTO);
        Assert.Equal(result, true);
    }
}