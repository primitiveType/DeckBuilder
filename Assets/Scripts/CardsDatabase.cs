using System;
using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;
using UnityEngine;

public class CardsDatabase
{
    private string CardsPath => Path.Combine(Application.streamingAssetsPath, "CardScripts");
    private string LuaExtension => ".lua";
    private Dictionary<string, Script> CardsByName = new Dictionary<string, Script>();

    private object Context;

    public CardsDatabase(object contextObj)
    {
        Context = contextObj;
        LoadAllCardPrototypes();
    }

    public Script GetCardScript(string cardName)
    {
        bool found = CardsByName.TryGetValue(cardName, out Script script);
        if (!found)
        {
            throw new ArgumentException($"Card with name {cardName} not found in database!");
        }

        return script;
    }

    private void LoadAllCardPrototypes()
    {
        DirectoryInfo cardsDirectory = new DirectoryInfo(CardsPath);

        LoadRecursively(cardsDirectory);

        void LoadRecursively(DirectoryInfo dir)
        {
            //TODO: look into moonsharp script loaders. We should be able to just provide a path instead of a string.
            foreach (DirectoryInfo childDir in dir.EnumerateDirectories())
            {
                LoadRecursively(childDir);
            }

            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (file.Extension.ToLower() == LuaExtension)
                {
                    string scriptString = File.ReadAllText(file.FullName);
                    AddCard(scriptString, file.Name.Substring(0, file.Name.IndexOf(file.Extension, StringComparison.Ordinal)));
                }
            }
        }
    }

    private void AddCard(string scriptString, string name)
    {
        var script = new Script();
        script.DoString(BaseCardTemplate);
        script.DoString(scriptString);
        InitializeScriptWithGlobalApi(script);

        CardsByName.Add(name, script);
    }

    private void InitializeScriptWithGlobalApi(Script script)
    {
        //TODO: look at managed object sharing so we don't have to specify ever function pointer
        script.Globals["game"] = Context;
    }

    public void AddCardScript(string scriptString, string name)
    {
        AddCard(scriptString, name);
    }
    
    private const string BaseCardTemplate =
        @"
        function getValidTargets(cardId) end
        function playCard(cardId, target) end
        function onDamageDealt(cardId, target, totalDamage, healthDamage) end
        function log(cardId) end
        function onThisCardPlayed(cardId) end
        function onAnyCardPlayed(cardId) end
        function cardInstanceCreate(cardId) 
        end
        function onCardCreated(cardId) end
        function onCardMoved(cardId) end
        function onThisCardPlayed(cardId) end
          ";
}