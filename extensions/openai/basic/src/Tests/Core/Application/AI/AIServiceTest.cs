namespace Core.Tests;
public class AIServiceTest
{
    public Application.Services.AI.Model.AI SetupCommand(Action add, Action update, Action delete, DpTest dpTest)
    {
        var domainAIMock = new Mock<Domain.Aggregates.AI.AI>();
        domainAIMock.Setup((o) => o.Add()).Callback(add);
        domainAIMock.Setup((o) => o.Update()).Callback(update);
        domainAIMock.Setup((o) => o.Delete()).Callback(delete);
        var ai = domainAIMock.Object;
        dpTest.MockDpDomain(ai);
        dpTest.Set<string>(ai, "Prompt", Faker.Lorem.Sentence(1));
        var applicationAIMock = new Mock<Application.Services.AI.Model.AI>();
        applicationAIMock.Setup((o) => o.ToDomain()).Returns(ai);
        var applicationAI = applicationAIMock.Object;
        return applicationAI;
    }
    public IAIService SetupApplicationService(DpTest dpTest)
    {
        var state = new Mock<IAIState>().Object;
        var aiService = new Application.Services.AI.AIService(state, dpTest.MockDp());
        return aiService;
    }
    [Fact]
    [Trait("ApplicationService", "AIService")]
    [Trait("ApplicationService", "Success")]
    public void Add_CommandNotNull_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var addCalled = false;
        var add = () =>
        {
            addCalled = true;
        };
        var command = SetupCommand(add, () =>
        {
        }, () =>
        {
        }, dpTest);
        var aiService = SetupApplicationService(dpTest);
        //Act
        aiService.Add(command);
        //Assert
        Assert.True(addCalled);
    }
    [Fact]
    [Trait("ApplicationService", "AIService")]
    [Trait("ApplicationService", "Success")]
    public void Update_CommandFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var updateCalled = false;
        var update = () =>
        {
            updateCalled = true;
        };
        var command = SetupCommand(() =>
        {
        }, update, () =>
        {
        }, dpTest);
        var aiService = SetupApplicationService(dpTest);
        //Act
        aiService.Update(command);
        //Assert
        Assert.True(updateCalled);
    }
    [Fact]
    [Trait("ApplicationService", "AIService")]
    [Trait("ApplicationService", "Success")]
    public void Delete_CommandFilled_Success()
    {
        //Arrange        
        var dpTest = new DpTest();
        var deleteCalled = false;
        var delete = () =>
        {
            deleteCalled = true;
        };
        var command = SetupCommand(() =>
        {
        }, () =>
        {
        }, delete, dpTest);
        var aiService = SetupApplicationService(dpTest);
        //Act
        aiService.Delete(command);
        //Assert
        Assert.True(deleteCalled);
    }
}