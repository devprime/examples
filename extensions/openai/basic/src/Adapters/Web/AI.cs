namespace DevPrime.Web;
public class AI : Routes
{
    public override void Endpoints(WebApplication app)
    {
        //Automatically returns 404 when no result  
        app.MapGet("/v1/ai", async (HttpContext http, IAIService Service, int? limit, int? offset, string ordering, string ascdesc, string filter) => await Dp(http).Pipeline(() => Service.GetAll(new Application.Services.AI.Model.AI(limit, offset, ordering, ascdesc, filter)), 404));
        //Automatically returns 404 when no result 
        app.MapGet("/v1/ai/{id}", async (HttpContext http, IAIService Service, Guid id) => await Dp(http).Pipeline(() => Service.Get(new Application.Services.AI.Model.AI(id)), 404));
        app.MapPost("/v1/ai", async (HttpContext http, IAIService Service, DevPrime.Web.Models.AI.AI command) => await Dp(http).Pipeline(() => Service.Add(command.ToApplication())));
        app.MapPut("/v1/ai", async (HttpContext http, IAIService Service, Application.Services.AI.Model.AI command) => await Dp(http).Pipeline(() => Service.Update(command)));
        app.MapDelete("/v1/ai/{id}", async (HttpContext http, IAIService Service, Guid id) => await Dp(http).Pipeline(() => Service.Delete(new Application.Services.AI.Model.AI(id))));
    }
}