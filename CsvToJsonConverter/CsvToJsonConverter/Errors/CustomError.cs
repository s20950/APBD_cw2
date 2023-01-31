namespace CsvToJsonConverter.Errors;

public enum ErrorType
{
    INFO, WARNING, ERROR
}
public class CustomError
{
    public ErrorType type { get; set; }
    public string? reason { get; set; }
    public object? data { get; set; }
}