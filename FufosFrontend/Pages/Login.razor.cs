

using FufosFrontend.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FufosFrontend.Pages;

public partial class Login : ComponentBase
{
    string Title {get; set;} = null!;
    string RegisterMessageText {get; set;} = null!;
    string RegisterText {get; set;} = null!;
    bool IsRegister {get; set;}
    bool PendingRequest {get; set;}

    [Inject] public UserAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] public NotificationService NotificationService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    public async Task OnSubmit(LoginArgs args)
    {
        if(PendingRequest)
            return;

        PendingRequest = true;

        StateHasChanged();

        bool Result;

        if(!IsRegister)
            Result = await AuthenticationService.Login(args.Username, args.Password);
        else
            Result = await AuthenticationService.Register(args.Username, args.Password);

        if(Result)
        {
            NotificationService.Notify(new()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Inicio de sesion",
                Detail = "Ha iniciado sesion con exito",
                Duration = 4000
            });

            NavigationManager.NavigateTo("/", forceLoad: true, replace: true);
        }else
        {
            NotificationService.Notify(new()
            {
                Severity = NotificationSeverity.Error,
                Summary = "Inicio de sesion",
                Detail = "No pudo iniciar sesion",
                Duration = 4000
            });
        }

        PendingRequest = false;
        StateHasChanged();
    }

    public void Register()
    {
        if(PendingRequest)
            return;
            
        IsRegister = !IsRegister;

        if(IsRegister)
            OnRegister();
        else
            OnLogin();

        StateHasChanged();
    }
}