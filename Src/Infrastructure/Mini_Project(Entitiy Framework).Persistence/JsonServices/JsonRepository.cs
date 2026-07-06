using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mini_Project_Entitiy_Framework_.Persistence.JsonServices
{
    public class JsonRepository<T>
    {
        public readonly string _path;

        public JsonRepository(string relativePathFromSolutionRoot)
        {
            string solutionRoot = FindSolutionRoot(AppDomain.CurrentDomain.BaseDirectory)
                                   ?? AppDomain.CurrentDomain.BaseDirectory;

            _path = Path.Combine(solutionRoot, relativePathFromSolutionRoot);
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        }
        private static string? FindSolutionRoot(string startDir)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null)
            {
                if (dir.GetFiles("*.slnx").Length > 0 || dir.GetFiles("*.sln").Length > 0)
                    return dir.FullName;

                dir = dir.Parent;
            }

            return null;
        }

        public virtual void Serialize(List<T> items)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter> { new StringEnumConverter() },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                File.WriteAllText(_path, JsonConvert.SerializeObject(items, settings));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[JSON Backup xəbərdarlığı]: {ex.Message}");
                Console.ResetColor();
            }
        }

        public virtual List<T> Deserialize()
        {
            try
            {
                if (!File.Exists(_path))
                    return new List<T>();

                string json = File.ReadAllText(_path);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<T>();

                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new StringEnumConverter() },
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                return JsonConvert.DeserializeObject<List<T>>(json, settings) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[JSON Backup xəbərdarlığı]: {ex.Message}");
                Console.ResetColor();
                return new List<T>();
            }
        }
    }
}
