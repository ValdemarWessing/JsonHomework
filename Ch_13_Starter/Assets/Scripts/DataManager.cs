using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class DataManager : MonoBehaviour, IManager
{
    private string _GroupMembers; // Path to GroupMembers_Data.txt
    private string _dataPath;
    private string _textFile;
    private string _streamingTextFile;
    private string _xmlLevelProgress;
    private string _xmlWeapons;
    private List<Weapon> weaponInventory = new List<Weapon>
    {
        new Weapon("Sword of Doom", 100),
        new Weapon("Butterfly knives", 25),
        new Weapon("Brass Knuckles", 15),
    };
    private string _jsonWeapons;
    private String _jsonGroupMembers;
    
    private List<GroupMembers> groupMembers = new List<GroupMembers> // List of GroupMembers
    {
        new GroupMembers("John Doe", "01/01/2000", "Blue"),
        new GroupMembers("Jane Doe", "02/02/2000", "Red"),
        new GroupMembers("Joe Doe", "03/03/2000", "Green"),
    };

    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    void Awake()
    {
        
            Debug.Log(Application.persistentDataPath);
        
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);
        
        _GroupMembers = _dataPath + "GroupMembers_Data.xml"; // Path for GoupMembers Data
        _jsonGroupMembers = _dataPath + "GroupMembersJSON.json";
        
        _textFile = _dataPath + "Save_Data.txt";
        _streamingTextFile = _dataPath + "Streaming_Save_Data.txt";
        _xmlLevelProgress = _dataPath + "Progress_Data.xml";
        _xmlWeapons = _dataPath + "WeaponInventory.xml";
        _jsonWeapons = _dataPath + "WeaponJSON.json";
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Data Manager initialized..";
        Debug.Log(_state);

        FilesystemInfo();
        NewDirectory();
        //DeleteDirectory();
        //NewTextFile();
        //UpdateTextFile();
        //ReadFromFile(_textFile);
        //WriteToStream(_streamingTextFile);
        //ReadFromFile(_streamingTextFile);
        //WriteToXML(_xmlLevelProgress);
        //ReadFromStream(_xmlLevelProgress);
        SerializeXML();
        DeserializeXML();
        SerializeJSON();
        DeserializeJSON();
    }

    public void FilesystemInfo()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}",
        Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }

    public void NewDirectory() //create a new directory
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists...");
            return;
        }

        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }

    public void DeleteDirectory()
    {
        if (!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist or has already been deleted..."); 
            return;
        }

        Directory.Delete(_dataPath, true);
        Debug.Log("Directory successfully deleted!");
    }

    public void NewTextFile()
    {
        if (File.Exists(_textFile))
        {
            Debug.Log("File already exists...");
            return;
        }

        File.WriteAllText(_textFile, "<SAVE DATA>\n");
        Debug.Log("New file created!");
    }

    public void UpdateTextFile()
    {
        if (!File.Exists(_textFile))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        File.AppendAllText(_textFile, $"Game started: {DateTime.Now}\n");
        Debug.Log("File updated successfully!");
    }

    public void ReadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        Debug.Log(File.ReadAllText(filename));
    }

    public void DeleteFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist or has already been deleted...");
            return;
        }

        File.Delete(_textFile);
        Debug.Log("File successfully deleted!");
    }

    public void WriteToStream(string filename)
    {
        if (!File.Exists(filename))
        {
            StreamWriter newStream = File.CreateText(filename);
            newStream.WriteLine("<Save Data> for HERO BORN \n");
            newStream.Close();

            Debug.Log("New file created with StreamWriter!");
        }

        StreamWriter streamWriter = File.AppendText(filename);
        streamWriter.WriteLine("Game ended: " + DateTime.Now);
        streamWriter.Close();

        Debug.Log("File contents updated with StreamWriter!");
    }

    public void ReadFromStream(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("File doesn't exist...");
            return;
        }

        StreamReader streamReader = new StreamReader(filename);
        Debug.Log(streamReader.ReadToEnd());
    }

    public void WriteToXML(string filename)
    {
        if (!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("level_progress");

            for (int i = 1; i < 5; i++)
            {
                xmlWriter.WriteElementString("level", "Level-" + i);
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
        }
    }

    public void SerializeXML() // Serialize GroupMembers Data
    {
        var xmlSerializer = new XmlSerializer(typeof(List<GroupMembers>));

        using (FileStream stream = File.Create(_GroupMembers))
        {
            xmlSerializer.Serialize(stream, groupMembers);
        }
    }

    public void DeserializeXML()
    {
        if (File.Exists(_GroupMembers))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<GroupMembers>));

            using (FileStream stream = File.OpenRead(_GroupMembers))
            {
                var groupMembers = (List<GroupMembers>)xmlSerializer.Deserialize(stream);
                foreach (var member in groupMembers)
                {
                    Debug.LogFormat("name: {0} - Date Of Birth: {1} - Favorite Color {2}", member.name, member.birthDay, member.favoriteColor);
                }
            }
        }
    }

    public void SerializeJSON()
    {
        //WeaponShop shop = new WeaponShop();
        //shop.inventory = weaponInventory;
       
        string jsonString = JsonUtility.ToJson(new GroupMembersWrapper { groupMembers = groupMembers }, true);
        using (StreamWriter stream = File.CreateText(_jsonGroupMembers))
        {
            stream.WriteLine(jsonString);
        }
        
    }

    public void DeserializeJSON()
    {
        if (File.Exists(_jsonGroupMembers))
        {
            using (StreamReader stream = new StreamReader(_jsonGroupMembers))
            {
                var jsonString = stream.ReadToEnd();
                var groupMembersWrapper = JsonUtility.FromJson<GroupMembersWrapper>(jsonString);
                groupMembers = groupMembersWrapper.groupMembers;

                foreach (var member in groupMembers)
                {
                    Debug.LogFormat("Json name: {0} - Json Date Of Birth: {1} - Json Favorite Color {2}", member.name, member.birthDay, member.favoriteColor);
                }
            }
        }
    }
}