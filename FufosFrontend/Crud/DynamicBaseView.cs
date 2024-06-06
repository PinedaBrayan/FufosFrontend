
using FufosFrontend.DTOS;
using FufosFrontend.Interfaces;
using FufosFrontend.Services;
using FufosFrontend.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Radzen;
using Fufos.Globals.Enums;
using FufosFrontend.Dataanotation;

namespace FufosFrontend.Crud;

public abstract class DynamicBaseView : ComponentBase
{
    [Parameter] public string ViewRoute { get; set; } = null!;
    [Parameter] public object Rowid {get; set;} = default!;
    [Inject] public ApiService ApiService {get; set;} = null!;
    [Inject] public IServiceProvider ServiceProvider { get; set; } = null!;
    [Inject] public TranslatorService TranslatorService {get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ModalError ModalError {get; set;} = null!;
    [Inject] public DialogService DialogService {get; set; } = null!;
    [Inject] public NotificationService NotificationService {get; set; } = null!;
    protected bool ViewIsReady {get; set;}
    protected bool RouteIsValid {get; set;}
    protected IBaseModule Module { get; set; } = null!;
    protected string LastViewRoute {get; set;} = null!;
    public object Entity {get; set;} = null!;
    public string FormId {get; set;} = $"{Guid.NewGuid()}";
    public EditContext EditFormContext {get; set;} = null!;
    protected bool IsBusy {get; set;}
    public abstract ViewType ViewType {get; set;}
    protected bool IgnoreCreate {get; set;}

    protected void SearchModule()
    {
        var Type = Utilities.SearchModule(ViewRoute);
        Module = (IBaseModule) ActivatorUtilities.CreateInstance(ServiceProvider, Type);

        IgnoreCreate = Type.GetCustomAttributes(typeof(IgnoreCreateAttribute), false).FirstOrDefault() != null;
    }

    protected void SetEditForm()
    {
        EditFormContext ??= new EditContext(this);
    }

    protected void SetEntity()
    {
        Entity = Module.GetType()
            .GetProperty("Entity")!
            .GetValue(Module)!;
    }

    protected void NavigateToList()
    {
        NavigationManager.NavigateTo(Module.GetModuleName());
    }

    protected void HandleInvalidSubmit()
    {

    }

    protected async Task HandleValidSubmit()
    {
        IsBusy = true;
        var Result = EditFormContext.Validate();
        
        if(!Result)
        {
            await ModalError.ShowModal(DialogService, "Error", "Falló el formulario");
            IsBusy = false;
            return;
        }

        var Request = await Module.Save();

        if(Request.Success)
        {
            NavigateToList();
            NotificationService.Notify(new () 
            { 
                Severity = NotificationSeverity.Success, 
                Summary = TranslatorService.GetMessage($"Enum.ViewType.{ViewType}"), 
                Detail = TranslatorService.GetMessage($"Enum.ViewType.{ViewType}.Description"), 
                Duration = 4000 
            });
            return;
        }

        try
        {
            var DictionaryResponse = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(Request.Result)!;
            await ModalError.ShowModal(DialogService, "Ocurrió un error", string.Join(" \n ", DictionaryResponse.Select(x => string.Join("\n",x.Value))));
        }
        catch
        {
            var Response = JsonConvert.DeserializeObject<BadRequestResponse>(Request.Result)!;
            await ModalError.ShowModal(DialogService, Response.Title, string.Join(" \n ", Response.Errors.Select(x => string.Join("\n",x.Value))));
        }

        IsBusy = false;
    }
}