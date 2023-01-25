using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Octokit;
using System.Net;
using System.Runtime.CompilerServices;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string process = args[0].Replace(".exe", "");
                process = process.Remove(0, process.LastIndexOf("\\") + 1);

                Console.WriteLine("Terminating process...");
                while (Process.GetProcessesByName(process).Length > 0)
                {
                    Console.WriteLine(Process.GetProcessesByName(process).Length + " processes remaining...");
                    Process[] myProcesses2 = Process.GetProcessesByName(process);
                    for (int i = 1; i < myProcesses2.Length; i++) { myProcesses2[i].Kill(); }

                    Thread.Sleep(300);
                }
                Console.WriteLine("Process terminated");
                GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("prak_G_13_Client"));
                var releases = gitHubClient.Repository.Release.GetLatest("DXR1DXR", "prak_G_13_Client");
                var latestVersionUrl = "https://github.com/DXR1DXR/prak_G_13_Client/releases/download/" + releases.Result.TagName + "/prak_G_13_Client.exe";
                var exePath = Directory.GetCurrentDirectory() + "\\prak_G_13_Client.exe";
                using (var wc = new WebClient())
                {
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "MyUserAgent");
                    
                    wc.DownloadFile(latestVersionUrl, exePath);
                }
                Console.WriteLine("File downloaded");
                File.Move(exePath, args[0], true);
                Console.WriteLine("Starting application...");
                Process.Start(args[0]);
                Environment.Exit(0);
            }
            catch {
                Console.WriteLine("Update Failed");
                Console.ReadKey();
            }
            
        }
    }
}