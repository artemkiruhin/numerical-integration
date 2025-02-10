using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NumericalIntegration.Core.Models;
using NumericalIntegration.Core.Services.Integration;

namespace NumericalIntegration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegrationController : ControllerBase
{
    [HttpPost("calculate")]
    public ActionResult<IntegrationResult> Calculate(IntegrationRequest request)
    {
        try
        {
            var calculator = new NumericalIntegrator(request.Function, request.A, request.B, request.N);
                
            var result = new IntegrationResult
            {
                TrapezoidalResult = calculator.TrapezoidalMethod(),
                SimpsonResult = calculator.SimpsonMethod(),
                GaussianResult = calculator.GaussianQuadrature(),
                ExactValue = calculator.CalculateExactValue(),
                Errors = calculator.CalculateErrors()
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}