using System.Net;
using System.Text;
using System.Text.Json;



namespace ATARAXIA;

public class HandleData
{



    public static void AddItemToStorage(string child, object data)
    {

        var obj = JsonSerializer.Serialize(data);
        AddToStorage(child, obj);
    }

    public static bool AddToListInStorage<T>(string child, T data)
    {

        var dat = GetData(child);
        if (dat != string.Empty)
        {
            // var listttype =	AppMare_Maui.SourceGenerationContext.Default.GetTypeInfo(List<>)
            var obj = JsonSerializer.Deserialize<List<T>>(dat);
            obj.Add(data);

            var fin = JsonSerializer.Serialize(obj);
            AddToStorage(child, fin);
        }
        else
        {
            var fin = JsonSerializer.Serialize(new List<T> { data });
            AddToStorage(child, fin);
        }

        return true;
    }

    private const string AppDataDirectoryName = "CashierData";

    /// <summary>
    /// Finds the first suitable, writable storage path for the application data.
    /// It prefers fixed drives (HDD, SSD) to avoid removable media.
    /// </summary>
    /// <returns>The full path to the application's data directory.</returns>
    /// <exception cref="IOException">Thrown if no suitable writable drive is found.</exception>
    private static string FindStoragePathForWriting()
    {
        // Get all drives on the system.
        var allDrives = DriveInfo.GetDrives();

        // Prefer fixed drives (like C:, D:) over removable ones for stability.
        var fixedDrives = allDrives.Where(d => d.IsReady && d.DriveType == DriveType.Fixed);

        foreach (var drive in fixedDrives)
        {
            try
            {
                var storagePath = Path.Combine(drive.Name, AppDataDirectoryName);

                // Ensure the directory exists. This also tests for write permissions.
                Directory.CreateDirectory(storagePath);

                // If we successfully created it (or it already existed), this is our path.
                Console.WriteLine($"Using storage path: {storagePath}");
                return storagePath;
            }
            catch (Exception ex)
            {
                // Couldn't write to this drive (e.g., permissions error), try the next one.
                Console.WriteLine($"Could not use drive {drive.Name}. Reason: {ex.Message}");
            }
        }

        // If no fixed drives worked, we could optionally try other drive types here.
        // For now, we'll throw an error.
        throw new IOException($"Could not find a suitable drive to store data. Please check drive permissions.");
    }

    /// <summary>
    /// Saves a key-value pair to a file in the application's data directory on a suitable partition.
    /// </summary>
    /// <param name="key">The filename (without extension).</param>
    /// <param name="value">The string content to save in the file.</param>
    public static async void AddToStorage(string key, string value)
    {
        try
        {
            // Find a suitable directory to save the file.
            string storageDirectory = FindStoragePathForWriting();

            // Combine the directory path with the key to get the full file path.
            var filePath = Path.Combine(storageDirectory, key);

            // Write the file asynchronously.
            await File.WriteAllTextAsync(filePath, value, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            // Log the error or handle it as needed.
            Console.WriteLine($"Error saving data for key '{key}': {ex.Message}");
            // Optionally, re-throw the exception if the calling code needs to know about the failure.
            // throw;
        }
    }

    /// <summary>
    /// Searches all available partitions for the data file and returns its content.
    /// </summary>
    /// <param name="key">The filename to search for.</param>
    /// <returns>The content of the file if found; otherwise, string.Empty.</returns>
    public static string GetData(string key)
    {
        string largestFilePath = string.Empty;
        long maxSize = -1; // Use -1 to ensure the first file found (even if 0 bytes) is chosen

        var allDrives = DriveInfo.GetDrives();

        // Search every drive that is ready to be accessed.
        foreach (var drive in allDrives.Where(d => d.IsReady))
        {
            try
            {
                var potentialPath = Path.Combine(drive.Name, AppDataDirectoryName, key);

                if (File.Exists(potentialPath))
                {
                    // Get file information to check its size.
                    var fileInfo = new FileInfo(potentialPath);
                    long currentSize = fileInfo.Length;
                    
                    Console.WriteLine($"Found potential file at: {potentialPath} (Size: {currentSize} bytes)");

                    // If this file is larger than the largest one we've found so far...
                    if (currentSize > maxSize)
                    {
                        // ...update our records.
                        maxSize = currentSize;
                        largestFilePath = potentialPath;
                        Console.WriteLine($"New largest file found: {largestFilePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore drives we can't access and continue.
                Console.WriteLine($"Could not scan drive {drive.Name}. Reason: {ex.Message}");
            }
        }

        // After checking all drives, if we found at least one file...
        if (!string.IsNullOrEmpty(largestFilePath))
        {
            // ...read the content of the largest one and return it.
            Console.WriteLine($"Returning content from the largest file: {largestFilePath}");
            return File.ReadAllText(largestFilePath, Encoding.UTF8);
        }

        // If the loop completes without finding any matching file, return empty.
        Console.WriteLine($"Data for key '{key}' not found on any drive.");
        return string.Empty;
    }

    // public static async Task<bool> RemoveFileFromListInStorage(string child, string id)
    // {
    //     var dat =getdata(child);
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
    //          AddToStorage(child, fin);
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
            var val = GetData(child);
            if (val != string.Empty)
            {

                var obj = JsonSerializer.Deserialize<T>(val);
                return obj;
            }

            return default;
        }
    }
}
