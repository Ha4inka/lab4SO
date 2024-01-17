using System;
using System.Collections.Generic;
using System.IO;

namespace lab4SO
{
    class ImportantFilesManager
    {
        private const string IMPORTANT_FILES_DB = "important_files.txt";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dotnet run <command> [options]");
                return;
            }

            string command = args[0];
            switch (command)
            {
                case "mark":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: dotnet run mark <file_path>");
                        return;
                    }
                    MarkAsImportant(args[1]);
                    break;

                case "unmark":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: dotnet run unmark <file_path>");
                        return;
                    }
                    UnmarkAsImportant(args[1]);
                    break;

                case "find":
                    string directory = ".";
                    string ext = null;
                    string nameContains = null;
                    for (int i = 1; i < args.Length; i += 2)
                    {
                        switch (args[i])
                        {
                            case "--dir":
                                directory = args[i + 1];
                                break;

                            case "--ext":
                                ext = args[i + 1];
                                break;

                            case "--name-contains":
                                nameContains = args[i + 1];
                                break;
                        }
                    }
                    FindImportantFiles(directory, ext, nameContains);
                    break;

                default:
                    Console.WriteLine("Unknown command: " + command);
                    break;
            }
        }

        static void MarkAsImportant(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(IMPORTANT_FILES_DB, true))
                {
                    writer.WriteLine(filePath);
                    Console.WriteLine("Marked as important: " + filePath);
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Error marking file as important: " + e.Message);
            }
        }

        static void UnmarkAsImportant(string filePath)
        {
            try
            {
                List<string> lines = new List<string>();
                using (StreamReader reader = new StreamReader(IMPORTANT_FILES_DB))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Trim().Equals(filePath))
                        {
                            lines.Add(line);
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter(IMPORTANT_FILES_DB))
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("Unmarked as important: " + filePath);
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Error unmarking file as important: " + e.Message);
            }
        }

        static void FindImportantFiles(string directory, string ext, string nameContains)
        {
            try
            {
                using (StreamReader reader = new StreamReader(IMPORTANT_FILES_DB))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string filePath = line.Trim();
                        FileInfo file = new FileInfo(filePath);
                        if (file.Exists && (ext == null || filePath.EndsWith("." + ext)) &&
                            (nameContains == null || filePath.Contains(nameContains)))
                        {
                            Console.WriteLine(filePath);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("Error finding important files: " + e.Message);
            }
        }
    }
}