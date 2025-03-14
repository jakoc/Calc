using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Calculator.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class CalculatorController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly SimpleCalculator _simpleCalculator;
        private readonly CachedCalculator _cachedCalculator;

        public CalculatorController(DatabaseService dbService)
        {
            _dbService = dbService;
            _simpleCalculator = new SimpleCalculator();
            _cachedCalculator = new CachedCalculator();
        }

        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Expression) || string.IsNullOrWhiteSpace(request.CalculatorType))
            {
                return BadRequest(new { error = "Ugyldig request. Mangler 'expression' eller 'calculatorType'" });
            }

            double result;
            try
            {
                result = request.CalculatorType == "cached"
                    ? EvaluateExpression(_cachedCalculator, request.Expression)
                    : EvaluateExpression(_simpleCalculator, request.Expression);
            }
            catch (Exception ex)
            {
                // Logfejl
                Console.WriteLine("Fejl under beregning: " + ex.Message);
                return BadRequest(new { error = "Beregning fejlede: " + ex.Message });
            }

            // Gem beregning i databasen
            _dbService.SaveCalculation(request.Expression, result);
            return Ok(new { expression = request.Expression, result });
        }


        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var history = _dbService.GetHistory();
            return Ok(history);
        }

        private static double EvaluateExpression(BascisCalculator calculator, string expression)
        {
            // Automatisk indsæt mellemrum omkring operatorer for at understøtte "5+3" formatet
            expression = expression.Replace("+", " + ")
                .Replace("-", " - ")
                .Replace("*", " * ")
                .Replace("/", " / ")
                .Trim();

            var tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 3)
                throw new ArgumentException("Ugyldigt format. Brug f.eks. '5 + 3'");

            if (!int.TryParse(tokens[0], out int a) || !int.TryParse(tokens[2], out int b))
                throw new ArgumentException("Kun heltal understøttes");

            // Brug af switch til at vælge operationen
            return tokens[1] switch
            {
                "+" => calculator.Add(a, b),
                "-" => calculator.Subtract(a, b),
                "*" => calculator.Multiply(a, b),
                "/" => calculator.Divide(a, b),
                _ => throw new ArgumentException("Ugyldig operator")
            };
        }
    }

    public class CalculationRequest
    {
        public string Expression { get; set; }
        public string CalculatorType { get; set; }
    }
}
