

namespace FufosFrontend.DTOS;

public class MenuItemDTO
{
    public Guid Id { get; set;} = Guid.NewGuid();
    public string Icon {get; set;} = null!;
    public string Name {get; set;} = null!;
    public string? Url {get; set;}
    public List<string> Path {get; set;} = [];

    public ICollection<MenuItemDTO>? SubMenu {get; set;}

    public object DeepCopy()
    {
        var Item = (MenuItemDTO)this.MemberwiseClone();

        Item.Path.Clear();

        return Item;
    }
}