using System;
using System.IO;

    public static class EnvFileReader
    {
        public static void LoadEnv(string path = ".env")
        {
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        Environment.SetEnvironmentVariable(key, value);
                    }
                }
            }
            else
            {
                Console.WriteLine(".env file not found. Make sure it exists in the project directory.");
            }
        }
    }


