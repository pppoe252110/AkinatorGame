using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance
    {
        get
        {
            if(instance == null)
                instance = FindFirstObjectByType<Database>();
            return instance;
        }
    }
    private static Database instance;

    public List<CharacterData> characters;

    private void Start()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Load()
    {
        var path = Path.Combine(Application.dataPath, "Characters.json");
        if (File.Exists(path))
        {
            var loadData = File.ReadAllText(Path.Combine(Application.dataPath, "Characters.json"));
            characters = JsonConvert.DeserializeObject<List<CharacterData>>(loadData);
        }
    }

    public void Save()
    {
        var saveData = JsonConvert.SerializeObject(characters);
        File.WriteAllText(Path.Combine(Application.dataPath, "Characters.json"), saveData);
    }
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string characterName;
    public string characterAttributes;
    public CharacterData(int id, string characterName, string characterAttributes)
    {
        this.id = id;
        this.characterName = characterName;
        this.characterAttributes = characterAttributes;
    }
}