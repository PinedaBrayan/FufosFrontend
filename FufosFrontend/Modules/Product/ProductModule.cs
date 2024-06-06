

using Fufos.Globals.Enums;
using FufosEntities.Entities;
using FufosFrontend.Components.Product;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Modules;

public class ProductModule(IServiceProvider serviceProvider) : BaseModule<Product>(serviceProvider)
{
    public override string PluralName {get; set;} = "Platos";
    public override string SingularName {get; set;} = "Plato";
    public override Dictionary<string, Type> GridColumns {get; set;} = new(){
        {nameof(Product.Name), typeof(Product).GetProperty("Name")!.PropertyType},
        {nameof(Product.Category), typeof(Product).GetProperty("Category")!.PropertyType}
    };

    public override RenderFragment GetForm() => (builder) =>{
        builder.OpenComponent<ProductForm>(0);
        builder.CloseComponent();
    };
}