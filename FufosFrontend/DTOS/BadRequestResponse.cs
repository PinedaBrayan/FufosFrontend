
namespace FufosFrontend.DTOS;

public record BadRequestResponse
{
    public string Title {get; set;} = null!;
    public int Status {get; set;}
    public Dictionary<string, List<string>> Errors {get; set;} = [];
}