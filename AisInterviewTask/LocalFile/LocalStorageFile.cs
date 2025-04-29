using System.Text.Json;
using AisInterviewTask.Models;

namespace AisInterviewTask.LocalFile
{
    public class LocalStorageFile    
    {
        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

        public static Files Load(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return new Files();

            var content = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<Files>(content) ?? new Files();
        }

        public static void Save(Files fileList, string jsonPath)
        {
            var content = JsonSerializer.Serialize(fileList, CachedJsonSerializerOptions);
            File.WriteAllText(jsonPath, content);
        }
    }
}
