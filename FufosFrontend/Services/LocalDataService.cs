using System.Collections.Concurrent;
using FufosFrontend.Utils;
using Newtonsoft.Json;

namespace FufosFrontend.Services;

public class LocalDataService
{
    private ConcurrentDictionary<string, dynamic> Data {get; set;} = [];

    public T GetLocalData<T>(string FileName)
    {
        var Found = Data.TryGetValue(FileName, out var Result);

        if(Found)
            return Result!;

        var Item = Utilities.SearchFile(FileName);

        if(string.IsNullOrEmpty(Item))
            return default!;

        Result = JsonConvert.DeserializeObject<T>(Item)!;

        Data.TryAdd(FileName, Result);

        return Result;
    }
}