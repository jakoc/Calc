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
                Console.WriteLine("Starting calculation for expression: " + request.Expression);
                result = request.CalculatorType == "cached"
                    ? EvaluateExpression(_cachedCalculator, request.Expression)
                    : EvaluateExpression(_simpleCalculator, request.Expression);
                Console.WriteLine("Calculation result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl under beregning: " + ex.Message); // Denne linje giver dig mere info om fejlen
                return BadRequest(new { error = "Beregning fejlede: " + ex.Message });
            }

            try
            {
                Console.WriteLine("Saving result to database: " + result);
                // Gem beregning i databasen
                _dbService.SaveCalculation(request.Expression, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl under gemning i database: " + ex.Message);
                return StatusCode(500, new { error = "Fejl under gemning i database: " + ex.Message });
            }

            return Ok(new { expression = request.Expression, result });
        }

        
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            try
            {
                Console.WriteLine("Henter historik..."); // Simple logning
                var history = _dbService.GetHistory();
                if (history == null || !history.Any())
                {
                    Console.WriteLine("Ingen historik fundet.");
                    return NotFound(new { error = "Ingen historik fundet" });
                }
                Console.WriteLine("Historik hentet: " + history.Count + " poster");
                return Ok(history);
            }
            catch (Exception ex)
            {
                // Log fejlen
                Console.WriteLine("Fejl ved hentning af historik: " + ex.Message);
                return StatusCode(500, new { error = "Fejl ved hentning af historik" });
            }
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
        public string? Expression { get; set; }
        public string? CalculatorType { get; set; }
    }
    
    
    
}
