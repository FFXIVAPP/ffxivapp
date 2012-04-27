// Project: AppModXIV
// File: ErrorLogging.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Collections.Generic;
using System.IO;

namespace AppModXIV.Classes
{
    public static class ErrorLogging
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="app"></param>
        /// <param name="path"></param>
        public static void LogError(string error, string app, string path)
        {
            var filename = path + "\\" + app + "_BugReport.txt";
            var data = new List<string>();
            if (File.Exists(filename))
            {
                using (var reader = new StreamReader(filename))
                {
                    string line;
                    do
                    {
                        line = reader.ReadLine();
                        data.Add(line);
                    }
                    while (line != null);
                }
            }
            var writeStart = 0;
            if (data.Count > 500)
            {
                writeStart = data.Count - 500;
            }
            using (var stream = new StreamWriter(filename, false))
            {
                for (var i = writeStart; i < data.Count; i++)
                {
                    stream.WriteLine(data[i]);
                }
                stream.Write(error);
            }
        }
    }
}