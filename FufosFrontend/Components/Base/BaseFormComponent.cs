
using Fufos.Globals.Enums;
using FufosFrontend.Crud;
using FufosFrontend.Services;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Components.Base;

// Clase base para los formularios de las tablas
public abstract class BaseFormComponent<T> : ComponentBase
{
    [Inject] public TranslatorService TranslatorService { get; set; } = null!;
    [CascadingParameter] public ViewType ViewType { get; set; }
    [CascadingParameter] public DynamicBaseView View {get; set;} = null!;
    protected T Entity {get; set;} = default!;

    protected void InitEntity()
    {
        Entity = (T) View.Entity;
    }

    protected override void OnInitialized()
    {
        InitEntity();
        base.OnInitialized();
    }
}