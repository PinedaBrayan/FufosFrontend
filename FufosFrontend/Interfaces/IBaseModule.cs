
using Fufos.Globals.Enums;
using FufosFrontend.DTOS;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Interfaces;

public interface IBaseModule
{
    string SingularName {get; set;}
    string PluralName { get; set;}
    Dictionary<string, Type> GridColumns {get; set;}
    Task<List<object>> GetData(params string[] foreignKeys);
    Task<bool> Delete(object Rowid);
    RenderFragment GetForm();
    string GetModuleName();
    Task<ApiResponse> Save();
    Task GetItem(object Rowid, params string[] foreignKeys);
}