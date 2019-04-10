// Copyright (C) Victor Derks. See LICENSE.md for the details of the software license.

using System;
using System.IO;
using System.Text;

namespace ConvertCppInclude
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ConvertCppInclude <directory> <pattern>");
                return;
            }

            try
            {
                const int directoryIndex = 0;
                const int patternIndex = 1;

                string[] filenames = Directory.GetFiles(args[directoryIndex], args[patternIndex], SearchOption.AllDirectories);
                Console.WriteLine("Found {0} files with pattern {1} in directory {2} ", filenames.Length, args[patternIndex], args[directoryIndex]);
                int convertCount = 0;

                foreach (string inputFilename in filenames)
                {
                    string outputFilename = inputFilename + ".converted";
                    bool linesStripped = ReadAndConvert(inputFilename, outputFilename);
                    if (linesStripped)
                    {
                        File.Delete(inputFilename);
                        File.Move(outputFilename, inputFilename);
                        convertCount++;
                    }
                    else
                    {
                        File.Delete(outputFilename);
                    }
                }

                Console.WriteLine("Converted {0} files", convertCount);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static bool ReadAndConvert(string inputPath, string outputPath)
        {
            bool lineConverted = false;
            string baseDirectory = Path.GetDirectoryName(inputPath);

            using (var input = new StreamReader(inputPath, Encoding.GetEncoding("Windows-1252"), true))
            using (var output = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                string line;
                while ((line = input.ReadLine()) != null)
                {
                    if (IsConvertNeeded(baseDirectory, line, out string convertedLine))
                    {
                        lineConverted = true;
                        output.WriteLine(convertedLine);
                    }
                    else
                    {
                        output.WriteLine(line);
                    }
                }
            }

            return lineConverted;
        }

        private static bool IsConvertNeeded(string baseDirectory, string line, out string convertedLine)
        {
            convertedLine = null;
            line = line.Trim();

            if (!line.Contains("#include") || !line.Contains("\""))
            {
                return false;
            }

            int start = line.IndexOf('"');
            int end = line.LastIndexOf('"');
            int length = end - (start + 1);
            if (start < 0 || end < 0 || end <= start)
            {
                return false;
            }

            string filename = line.Substring(start + 1, length);
            string path = Path.Combine(baseDirectory, filename);
            if (File.Exists(path))
            {
                return false;
            }

            var sb = new StringBuilder(line)
            {
                [start] = '<', [end] = '>'
            };
            convertedLine = sb.ToString();
            return true;
        }
    }
}
