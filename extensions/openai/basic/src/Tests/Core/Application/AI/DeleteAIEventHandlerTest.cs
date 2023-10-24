namespace Core.Tests;
public class DeleteAIEventHandlerTest
{
    public DeleteAI Create_AI_Object_OK(DpTest dpTest)
    {
        var ai = AITest.Create_AI_Required_Properties_OK(dpTest);
        var deleteAI = new DeleteAI();
        dpTest.SetDomainEventObject(deleteAI, ai);
        return deleteAI;
    }
    [Fact]
    [Trait("EventHandler", "DeleteAIEventHandler")]
    [Trait("EventHandler", "Success")]
    public void Handle_AIObjectFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        object parameter = null;
        var deleteAI = Create_AI_Object_OK(dpTest);
        var ai = dpTest.GetDomainEventObject<Domain.Aggregates.AI.AI>(deleteAI);
        var repositoryMock = new Mock<IAIRepository>();
        repositoryMock.Setup((o) => o.Delete(ai.ID)).Returns(true).Callback(() =>
        {
            parameter = ai;
        });
        var repository = repositoryMock.Object;
        var stateMock = new Mock<IAIState>();
        stateMock.SetupGet((o) => o.AI).Returns(repository);
        var state = stateMock.Object;
        var deleteAIEventHandler = new Application.EventHandlers.AI.DeleteAIEventHandler(state, dpTest.MockDp<IAIState>(state));
        //Act
        var result = deleteAIEventHandler.Handle(deleteAI);
        //Assert
        Assert.Equal(parameter, ai);
        Assert.Equal(result, true);
    }
}