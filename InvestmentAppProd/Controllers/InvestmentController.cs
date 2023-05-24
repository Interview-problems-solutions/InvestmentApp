using InvestmentAppProd.Commands.DeleteInvestment;
using InvestmentAppProd.Queries.FetchAllInvestments;
using InvestmentAppProd.Queries.FetchInvestment;

namespace InvestmentAppProd.Controllers
{
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
        public ActionResult<Investment> AddInvestment([FromBody] Investment investment)
        {
            try
            {
                if (investment.StartDate > DateTime.Now)
                    return BadRequest("Investment Start Date cannot be in the future.");

                investment.CalculateValue();
                _context.ChangeTracker.Clear();
                _context.Investments.Add(investment);
                _context.SaveChanges();

                return CreatedAtAction("AddInvestment", investment.Name, investment);
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
        public ActionResult UpdateInvestment([FromQuery] string name, [FromBody] Investment investment)
        {
            try
            {
                if (name != investment.Name)
                    return BadRequest("Name does not match the Investment you are trying to update.");

                if (investment.StartDate > DateTime.Now)
                    return BadRequest("Investment Start Date cannot be in the future.");

                investment.CalculateValue();
                _context.ChangeTracker.Clear();
                _context.Entry(investment).State = EntityState.Modified;
                _context.SaveChanges();

                return NoContent();
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
}
