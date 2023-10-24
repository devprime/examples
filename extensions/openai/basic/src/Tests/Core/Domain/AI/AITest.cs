namespace Core.Tests;
public class AITest
{
    public static Guid FixedID = new Guid("c1beb353-357c-4616-96f2-915b6a0a3860");

#region fixtures
    public static Domain.Aggregates.AI.AI Create_AI_Required_Properties_OK(DpTest dpTest)
    {
        var ai = new Domain.Aggregates.AI.AI();
        dpTest.MockDpDomain(ai);
        dpTest.Set<Guid>(ai, "ID", FixedID);
        dpTest.Set<string>(ai, "Prompt", Faker.Lorem.Sentence(1));
        return ai;
    }
    public static Domain.Aggregates.AI.AI Create_AI_With_Prompt_Required_Property_Missing(DpTest dpTest)
    {
        var ai = new Domain.Aggregates.AI.AI();
        dpTest.MockDpDomain(ai);
        dpTest.Set<Guid>(ai, "ID", FixedID);
        return ai;
    }

#endregion fixtures

#region add
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Success")]
    public void Add_Required_properties_filled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var ai = Create_AI_Required_Properties_OK(dpTest);
        dpTest.MockDpProcessEvent<bool>(ai, "CreateAI", true);
        dpTest.MockDpProcessEvent(ai, "AICreated");
        //Act
        ai.Add();
        //Assert
        var domainevents = dpTest.GetDomainEvents(ai);
        Assert.True(domainevents[0] is CreateAI);
        Assert.True(domainevents[1] is AICreated);
        Assert.NotEqual(ai.ID, Guid.Empty);
        Assert.True(ai.IsNew);
        Assert.True(ai.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Add")]
    [Trait("Aggregate", "Fail")]
    public void Add_Prompt_Missing_Fail()
    {
        //Arrange
        var dpTest = new DpTest();
        var ai = Create_AI_With_Prompt_Required_Property_Missing(dpTest);
        //Act and Assert
        var ex = Assert.Throws<PublicException>(ai.Add);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("Prompt is required", i));
        Assert.False(ai.Dp.Notifications.IsValid);
    }

#endregion add

#region update
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Success")]
    public void Update_Required_properties_filled_Success()
    {
        //Arrange        
        var dpTest = new DpTest();
        var ai = Create_AI_Required_Properties_OK(dpTest);
        dpTest.MockDpProcessEvent<bool>(ai, "UpdateAI", true);
        dpTest.MockDpProcessEvent(ai, "AIUpdated");
        //Act
        ai.Update();
        //Assert
        var domainevents = dpTest.GetDomainEvents(ai);
        Assert.True(domainevents[0] is UpdateAI);
        Assert.True(domainevents[1] is AIUpdated);
        Assert.NotEqual(ai.ID, Guid.Empty);
        Assert.True(ai.Dp.Notifications.IsValid);
    }
    [Fact]
    [Trait("Aggregate", "Update")]
    [Trait("Aggregate", "Fail")]
    public void Update_Prompt_Missing_Fail()
    {
        //Arrange
        var dpTest = new DpTest();
        var ai = Create_AI_With_Prompt_Required_Property_Missing(dpTest);
        //Act and Assert
        var ex = Assert.Throws<PublicException>(ai.Update);
        Assert.Equal("Public exception", ex.ErrorMessage);
        Assert.Collection(ex.Exceptions, i => Assert.Equal("Prompt is required", i));
        Assert.False(ai.Dp.Notifications.IsValid);
    }

#endregion update

#region delete
    [Fact]
    [Trait("Aggregate", "Delete")]
    [Trait("Aggregate", "Success")]
    public void Delete_IDFilled_Success()
    {
        //Arrange
        var dpTest = new DpTest();
        var ai = Create_AI_Required_Properties_OK(dpTest);
        dpTest.MockDpProcessEvent<bool>(ai, "DeleteAI", true);
        dpTest.MockDpProcessEvent(ai, "AIDeleted");
        //Act
        ai.Delete();
        //Assert
        var domainevents = dpTest.GetDomainEvents(ai);
        Assert.True(domainevents[0] is DeleteAI);
        Assert.True(domainevents[1] is AIDeleted);
    }

#endregion delete
}