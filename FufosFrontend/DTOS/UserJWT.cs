
namespace FufosFrontend.DTOS;

public class JWTUserDTO
{
    public int Rowid {get; set;}
    public string Email {get; set;} = string.Empty;
    public string FullName {get; set;} = string.Empty;
    public bool IsAdmin {get; set;}
    public bool IsEmployee {get; set;}
}