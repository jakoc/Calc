namespace Calculator.Controllers;

public interface IDatabaseService
{
    void SaveCalculation(string expression, double result);
    List<(string Expression, double Result, DateTime CreatedAt)> GetHistory();
}