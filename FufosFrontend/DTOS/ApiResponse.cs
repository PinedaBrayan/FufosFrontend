
namespace FufosFrontend.DTOS;

public record ApiResponse
{
    public bool Success {get; set;}
    public string Result {get; set;} = null!;
}