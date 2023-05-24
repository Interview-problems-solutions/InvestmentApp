namespace InvestmentAppProd.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class InvestmentController : Controller
{
    private readonly InvestmentDBContext _context;
    private readonly IMediator _mediator;

    public InvestmentController(InvestmentDBContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Investment>>> FetchInvestment()
    {
        try
        {
            return Ok(await _mediator.Send(new FetchAllInvestmentsCommand()));
        }
        catch (Exception e)
        {
            return NotFound(e.ToString());
        }
    }

    [HttpGet("name")]
    public async Task<ActionResult<Investment>> FetchInvestment([FromQuery] string name)
    {
        try
        {
            var investment = await _mediator.Send(new FetchInvestmentCommand(name));
            
            if (investment.HasNoValue) return NotFound($"Investment with name {name} not found.");

            return Ok(investment.Value);
        }
        catch (Exception e)
        {
            return NotFound(e.ToString());
        }
    }


    [HttpPost]
    public async Task<ActionResult<Investment>> AddInvestment([FromBody] Investment investment)
    {
        try
        {
            var result = await _mediator.Send(new AddInvestmentCommand(investment));

            return result.IsFailure switch
            {
                true when result.Error.ErrorType == ErrorType.StartDateInFuture => BadRequest(
                    "Investment Start Date cannot be in the future."),
                true when result.Error.ErrorType == ErrorType.AlreadyExists => BadRequest(
                    $"Investment with name {investment.Name} already exists."),
                _ => CreatedAtAction("AddInvestment", investment.Name, investment)
            };
        }
        catch (DbUpdateException dbE)
        {
            return Conflict(dbE.ToString());
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }
    }

    [HttpPut("name")]
    public async Task<ActionResult> UpdateInvestment([FromQuery] string name, [FromBody] Investment investment)
    {
        try
        {
            var result = await _mediator.Send(new UpdateInvestmentCommand(name, investment));

            return result.IsFailure switch
            {
                true when result.Error.ErrorType == ErrorType.NameMisMatch => BadRequest(
                    "Name does not match the Investment you are trying to update."),
                true when result.Error.ErrorType == ErrorType.StartDateInFuture => BadRequest(
                    "Investment Start Date cannot be in the future."),
                true when result.Error.ErrorType == ErrorType.DoesNotExit => BadRequest(
                    $"Investment with name {investment.Name} does not exists."),
                _ => NoContent()
            };
        }
        catch (Exception e)
        {
            return NotFound(e.ToString());
        }
    }

    [HttpDelete("name")]
    public async Task<ActionResult> DeleteInvestment([FromQuery] string name)
    {
        try
        {
            var result = await _mediator.Send(new DeleteInvestmentCommand(name));
            
            if (result.IsFailure)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString());
        }

    }
}
