using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;
using UnityEngine;

public class GlobalApi
{
    private static string CardsPath => Path.Combine(Application.streamingAssetsPath, "CardScripts");
    private static string LuaExtension => ".lua";
    private static Dictionary<string, Script> CardScriptsByName = new Dictionary<string, Script>();

    private static void Log(string str, Script script = null)
    {
        Debug.Log($"Lua : {str}");
    }

    private static void LoadAllCardPrototypes()
    {
        DirectoryInfo cardsDirectory = new DirectoryInfo(CardsPath);

        LoadRecursively(cardsDirectory);

        void LoadRecursively(DirectoryInfo dir)
        {
            foreach (DirectoryInfo childDir in dir.EnumerateDirectories())
            {
                LoadRecursively(childDir);
            }

            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (file.Extension.ToLower() == LuaExtension)
                {
                    string scriptString = File.ReadAllText(file.FullName);
                    var script = new Script();
                    script.DoString(scriptString);
                    InitializeScriptWithGlobalApi(script);
                    CardScriptsByName.Add(file.Name, script);
                }
            }
        }
    }

    public static int Mul(int a, int b, Script script = null)
    {
        return a * b;
    }
    private static void InitializeScriptWithGlobalApi(Script script)
    {
        script.Globals[nameof(Log)] = (Action<string, Script>) Log;
        script.Globals[nameof(Mul)] = (Func<int, int, Script, int>)Mul;
        
        var val = script.Call(script.Globals["fact"], 4);
        Debug.Log(val.Number);
        

        //TODO: wrap types we care about with common functions to abstract this ugly script.call(script.globals...) stuff.
        script.Call(script.Globals["log"], "Tester!");
    }

    public static void Initialize()
    {
        LoadAllCardPrototypes();
    }
}