﻿using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using LeagueSharp;

//partial copy pasterino from https://github.com/Hellsing/LeagueSharp/blob/master/Avoid/UpdateChecker.cs

namespace SparkTech
{
    public class UpdateChecker
    {
        internal static void LibraryUpdateCheck()
        {
            using (var client = new WebClient())
            {
                new Thread(async () =>
                {
                    try
                    {
                        var assemblyName = Assembly.GetExecutingAssembly().GetName();

                        var data =
                            await
                                // ReSharper disable once AccessToDisposedClosure
                                client.DownloadStringTaskAsync(
                                    "https://raw.github.com/Wiciaki/Releases/master/SparkTech/Properties/AssemblyInfo.cs");

                        //dumb code warning
                        var version =
                            System.Version.Parse(
                                new Regex("AssemblyFileVersion\\((\"(.+?)\")\\)").Match(data).Groups[1].Value
                                    .Replace("\"", ""));
                        //end dumb code warning

                        if (Settings.Debug)
                        {
                            Game.PrintChat(version.ToString());
                        }

                        if (version > assemblyName.Version)
                        {
                            Game.PrintChat("Updated version of the library is available: {1} => {2}",
                                assemblyName.Name,
                                assemblyName.Version,
                                version);
                        }
                        else if (version == assemblyName.Version)
                        {
                            Comms.Print("You are running the latest version of the library");
                        }
                        else if (version < assemblyName.Version)
                        {
                            Comms.Print("Error 1!");
                        }
                        else
                        {
                            Comms.Print("Error 2!");
                        }
                    }
                    catch
                    {
                        Comms.Print("Checking for an update FAILED!");
                    }
                }
                    ).Start();
            }
        }
    }
}