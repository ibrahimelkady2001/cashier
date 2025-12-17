using System.Net;
using System.Text.Json;



namespace ATARAXIA;

public class HandleData
{
  
 

    public static void AddItemToStorage(string child, object data)
    {
  
        var obj = JsonSerializer.Serialize(data);
        Preferences.Set(child, obj);
    }

    public static bool AddToListInStorage<T>(string child, T data)
    {
  
        var dat = Preferences.Get(child, string.Empty);
        if (dat != string.Empty)
        {
            // var listttype =	AppMare_Maui.SourceGenerationContext.Default.GetTypeInfo(List<>)
            var obj = JsonSerializer.Deserialize<List<T>>(dat);
            obj.Add(data);
          
            var fin = JsonSerializer.Serialize(obj);
            Preferences.Set(child, fin);
        }
        else
        {
            var fin = JsonSerializer.Serialize(new List<T> { data });
            Preferences.Set(child, fin);
        }

        return true;
    }

    // public static async Task<bool> RemoveFileFromListInStorage(string child, string id)
    // {
    //     var dat = Preferences.Get(child, string.Empty);
    //     if (dat != string.Empty)
    //     {
    //         var obj = JsonSerializer.Deserialize<List<FileSrcInApp>>(dat,
    //             SourceGenerationContext.Default.ListFileSrcInApp);
    //         var o = 0;
    //         var u = false;
    //         foreach (var x in obj)
    //         {
    //             if (x.ID == id)
    //             {
    //                 u = true;
    //                 if (x.Path != null)
    //                 {
    //                     if (File.Exists(x.Path)) await FileHelper.DeleteFileAsync(x.Path);
    //                     break;
    //                 }
    //             }

    //             o++;
    //         }

    //         if (u) obj.RemoveAt(o);
    //         var fin = JsonSerializer.Serialize(obj, SourceGenerationContext.Default.ListFileSrcInApp);
    //         Preferences.Set(child, fin);
    //     }

    //     return true;
    // }

    public static class GetElement<T>
    {
        // public static async Task<T> GetFirebaseChild(string child)
        // {
        //     var config = Getfire();
        //     var client = new FirebaseClient(config);
        //     var li = await client.GetAsync(child);
        //     return li.ResultAs<T>();
        // }

        public static T GetItem(string child)
        {
            var val = Preferences.Get(child, string.Empty);
            if (val != string.Empty)
            {
              
                var obj = JsonSerializer.Deserialize<T>(val);
                return obj;
            }

            return default;
        }
    }
}