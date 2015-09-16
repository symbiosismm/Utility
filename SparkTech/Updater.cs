﻿// Taken from https://github.com/Hellsing/LeagueSharp/blob/master/Avoid/UpdateChecker.cs
// Which was c+p'd too, in fact ¯\_(ツ)_/¯

namespace SparkTech
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Diagnostics.CodeAnalysis;

 // using static Comms;

    [SuppressMessage("ReSharper", "UseStringInterpolation")] // Remove when .NET 4.6 arrives
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public static class Updater
    {
        public static void Check(string gitName)
         { 
             using (var client = new WebClient()) 
             { 
                 new Thread(async () =>
                 { 
                     try 
                     { 
                         var data = await client.DownloadStringTaskAsync(string.Format("https://raw.github.com/Wiciaki/Releases/master/{0}/Properties/AssemblyInfo.cs", gitName)); 
                         var version = Version.Parse(new Regex("AssemblyFileVersion\\((\"(.+?)\")\\)").Match(data).Groups[1].Value.Replace("\"", "")); 
                         var assemblyName = Assembly.GetExecutingAssembly().GetName();
                         if (version == assemblyName.Version)
                         {
                             if (!Settings.SkipNoUpdate)
                             {
                                 Comms.Print(string.Format("You are using the latest version of {0}", gitName));
                             }
                         }
                         else if (version != assemblyName.Version)
                         {
                             Comms.Print(string.Format("{0} - a new version is available! ", gitName));
                             Comms.Print(string.Format("{0} => {1}", assemblyName.Version, version));
                         }
                         else
                         {
                             Comms.Print(string.Format("{0} - Checking for an update FAILED!", gitName), true);
                         }
                         Comms.Print(string.Format("SparkTech - {0} - Finished update checking.", gitName), true);
                     }
                     catch (Exception ex)
                     {
                         Comms.Print("Checking for an update FAILED! Exception: ", true);
                         Comms.Print(ex.ToString(), true);
                     }
                 }).Start(); 
             } 
         }

        internal static void CheckLibrary()
        {
            using (WebClient client = new WebClient())
            {
                new Thread(async () =>
                {
                    try
                    {
                        AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                        Comms.Print("Assembly Name: " + assemblyName, true);
                        string data = await client.DownloadStringTaskAsync("https://raw.github.com/Wiciaki/Releases/master/SparkTech/Properties/AssemblyInfo.cs");
                        var version = Version.Parse(new Regex("AssemblyFileVersion\\((\"(.+?)\")\\)").Match(data).Groups[1].Value.Replace("\"", ""));
                        if (version == assemblyName.Version)
                        {
                            if (!Settings.SkipNoUpdate)
                            {
                                Comms.Print("You are using the latest version of the L# Spaghetti.");
                            }
                        }
                        else if (version != assemblyName.Version)
                        {
                            Comms.Print(string.Format("A new spaghetti sauce is available: {0} => {1}", assemblyName.Version, version));
                        }
                        else
                        {
                            Comms.Print("Checking for a library update FAILED! (else)", true);
                        }
                        Comms.Print("SparkTech - Updater - Finished lib update checking.", true);
                    }
                    catch (Exception ex)
                    {
                        Comms.Print("Checking for a library update FAILED! Exception: ", true);
                        Comms.Print(ex.ToString(), true);
                    }
                }).Start();
            }
        }
    }
}