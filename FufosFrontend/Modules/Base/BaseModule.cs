using Fufos.Globals.Enums;
using FufosFrontend.DTOS;
using FufosFrontend.Interfaces;
using FufosFrontend.Services;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Modules;

public abstract class BaseModule<T>(IServiceProvider serviceProvider) : IBaseModule where T : class
{
    public T Entity {get; set;} = Activator.CreateInstance<T>();
    public abstract string PluralName {get; set;}
    public abstract string SingularName {get; set;}
    public abstract Dictionary<string, Type> GridColumns {get; set;}
    public string ModuleName => GetType().Name.Replace("Module", "");
    protected ApiService ApiService => GetApiService();

    public abstract RenderFragment GetForm();

    public string GetModuleName() => ModuleName;

    protected ApiService GetApiService() => serviceProvider.GetService<ApiService>()!;

    public virtual async Task<List<object>> GetData(params string[] foreignKeys)
    {
        var Result = await ApiService.GetAll<T>(ModuleName, foreignKeys);
        return Result.Cast<object>()
            .ToList();
    }

    public virtual async Task<bool> Delete(object Rowid)
    {
        var Result = await ApiService.Delete(ModuleName, Rowid);
        return Result;
    }

    public virtual async Task<ApiResponse> Save()
    {
        var Rowid = typeof(T).GetProperty("Rowid")?
            .GetValue(Entity);

        ApiResponse Request;

        if (Rowid is null || Convert.ToInt64(Rowid) == 0)
            Request = await ApiService.Create(ModuleName, Entity);
        else
            Request = await ApiService.Update(ModuleName, Entity);

        return Request;
    }

    public virtual async Task GetItem(object Rowid, params string[] foreignKeys)
    {
        Entity = await ApiService.GetItem<T>(ModuleName, Rowid, foreignKeys) ?? Activator.CreateInstance<T>();
    }
}