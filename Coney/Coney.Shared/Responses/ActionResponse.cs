namespace Coney.Shared.Responses;

public class ActionResponse<T>
{
    public bool Status { get; set; }
    public int Code { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }
}