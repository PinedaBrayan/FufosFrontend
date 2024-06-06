

using Fufos.Globals.Enums;
using FufosEntities.Entities;
using FufosFrontend.Components.Category;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Modules;

public class CategoryModule(IServiceProvider serviceProvider) : BaseModule<Category>(serviceProvider)
{
    public override string PluralName {get; set;} = "Catálogos";
    public override string SingularName {get; set;} = "Catálogo";
    public override Dictionary<string, Type> GridColumns {get; set;} = new(){
        {nameof(Category.Name), typeof(Category).GetProperty("Name")!.PropertyType}
    };

    public override RenderFragment GetForm() => (builder) =>{
        builder.OpenComponent<CategoryForm>(0);
        builder.CloseComponent();
    };
}