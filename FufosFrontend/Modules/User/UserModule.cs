

using FufosEntities.Entities;
using FufosFrontend.Components.User;
using FufosFrontend.Dataanotation;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Modules;

[IgnoreCreate]
public class UserModule(IServiceProvider serviceProvider) : BaseModule<User>(serviceProvider)
{
    public override string PluralName {get; set;} = "Usuarios";
    public override string SingularName {get; set;} = "Usuario";
    public override Dictionary<string, Type> GridColumns {get; set;} = new(){
        {nameof(User.FullName), typeof(User).GetProperty("FullName")!.PropertyType},
        {nameof(User.Status), typeof(User).GetProperty("Status")!.PropertyType},
        {nameof(User.IsAdmin), typeof(User).GetProperty("IsAdmin")!.PropertyType},
    };

    public override RenderFragment GetForm() => (builder) =>{
        builder.OpenComponent<UserForm>(0);
        builder.CloseComponent();
    };
}