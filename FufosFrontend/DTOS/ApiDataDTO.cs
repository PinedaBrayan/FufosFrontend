
namespace FufosFrontend.DTOS;

public record ApiDataDTO
{
    public string Name {get; set;} = null!;
    public string Url {get; set;} = null!;
    public List<string> EndPoints {get; set;} = [];
    public bool Authentication {get; set;}
}