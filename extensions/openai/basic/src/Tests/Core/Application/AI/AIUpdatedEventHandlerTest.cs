namespace Core.Tests;
public class AIUpdatedEventHandlerTest
{
    public Dictionary<string, string> CustomSettings()
    {
        var settings = new Dictionary<string, string>();
        settings.Add("stream.aievents", "aievents");
        return settings;
    }
    private AIUpdatedEventDTO SetEventData(Domain.Aggregates.AI.AI ai)
    {
        return new AIUpdatedEventDTO()
        {ID = ai.ID, Prompt = ai.Prompt};
    }
    public AIUpdated Create_AI_Object_OK(DpTest dpTest)
    {
        var ai = AITest.Create_AI_Required_Properties_OK(dpTest);
        var aiUpdated = new AIUpdated();
        dpTest.SetDomainEventObject(aiUpdated, ai);
        return aiUpdated;
    }
    [Fact]
    [Trait("EventHandler", "AIUpdatedEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_AIObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var settings = CustomSettings();
        var aiUpdated = Create_AI_Object_OK(dpTest);
        var ai = dpTest.GetDomainEventObject<Domain.Aggregates.AI.AI>(aiUpdated);
        var aiUpdatedEventHandler = new Application.EventHandlers.AI.AIUpdatedEventHandler(null, dpTest.MockDp<IAIState>(null));
        dpTest.SetupSettings(aiUpdatedEventHandler.Dp, settings);
        dpTest.SetupStream(aiUpdatedEventHandler.Dp);
        //Act
        var result = aiUpdatedEventHandler.Handle(aiUpdated);
        //Assert
        var sentEvents = dpTest.GetSentEvents(aiUpdatedEventHandler.Dp);
        var aiUpdatedEventDTO = SetEventData(ai);
        Assert.Equal(sentEvents[0].Destination, settings["stream.aievents"]);
        Assert.Equal("AIUpdated", sentEvents[0].EventName);
        Assert.Equivalent(sentEvents[0].EventData, aiUpdatedEventDTO);
        Assert.Equal(result, true);
    }
}