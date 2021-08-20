using System;
using System.Collections.Generic;
using System.IO;
using Data;

public class CardsDatabase
{
    private string CardsPath => "CardScripts";
    private string LuaExtension => ".lua";
    private Dictionary<string, GameEntity> CardsByName = new Dictionary<string, GameEntity>();

    private object Context;

    public CardsDatabase(object contextObj)
    {
        Context = contextObj;
        LoadAllCardPrototypes();
    }

    private void LoadAllCardPrototypes()
    {
        
    }

   
}