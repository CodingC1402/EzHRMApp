using System.IO;
using System.Text.Json;

namespace DAL.Others
{
    public static class JsonUtils
    {
        public static T DeserialLize<T>(string fileName)
        {
            if (!File.Exists(fileName))
                return default(T);

            string json = File.ReadAllText(fileName);
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch { return default(T); }
        }

        public static bool Serialize(object obj, string fileName)
        {
            try
            {
                string json = JsonSerializer.Serialize(obj);
                File.WriteAllText(fileName, json);
                return true;
            }
            catch { return false; }
        }
    }
}
