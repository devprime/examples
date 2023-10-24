namespace DevPrime.Web.Models.AI;
public class AI
{
    public string Prompt { get; set; }
    public static Application.Services.AI.Model.AI ToApplication(DevPrime.Web.Models.AI.AI aI)
    {
        if (aI is null)
            return new Application.Services.AI.Model.AI();
        Application.Services.AI.Model.AI _aI = new Application.Services.AI.Model.AI();
        _aI.Prompt = aI.Prompt;
        return _aI;
    }
    public static List<Application.Services.AI.Model.AI> ToApplication(IList<DevPrime.Web.Models.AI.AI> aIList)
    {
        List<Application.Services.AI.Model.AI> _aIList = new List<Application.Services.AI.Model.AI>();
        if (aIList != null)
        {
            foreach (var aI in aIList)
            {
                Application.Services.AI.Model.AI _aI = new Application.Services.AI.Model.AI();
                _aI.Prompt = aI.Prompt;
                _aIList.Add(_aI);
            }
        }
        return _aIList;
    }
    public virtual Application.Services.AI.Model.AI ToApplication()
    {
        var model = ToApplication(this);
        return model;
    }
}