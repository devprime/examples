namespace Core.Tests;
public class UpdateAIEventHandlerTest
{
    public UpdateAI Create_AI_Object_OK(DpTest dpTest)
    {
        var ai = AITest.Create_AI_Required_Properties_OK(dpTest);
        var updateAI = new UpdateAI();
        dpTest.SetDomainEventObject(updateAI, ai);
        return updateAI;
    }
    [Fact]
    [Trait("EventHandler", "UpdateAIEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_AIObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        object parameter = null;
        var updateAI = Create_AI_Object_OK(dpTest);
        var ai = dpTest.GetDomainEventObject<Domain.Aggregates.AI.AI>(updateAI);
        var repositoryMock = new Mock<IAIRepository>();
        repositoryMock.Setup((o) => o.Update(ai)).Returns(true).Callback(() =>
        {
            parameter = ai;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IAIState>();
        stateMock.SetupGet((o) => o.AI).Returns(repository);
        var state = stateMock.Object;
        var updateAIEventHandler = new Application.EventHandlers.AI.UpdateAIEventHandler(state, dpTest.MockDp<IAIState>(state));
        //Act
        var result = updateAIEventHandler.Handle(updateAI);
        //Assert
        Assert.Equal(parameter, ai);
        Assert.Equal(result, true);
    }
}