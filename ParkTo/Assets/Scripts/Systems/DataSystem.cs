using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
using System.Linq;

public class DataSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static DataSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    #endregion

    //private static readonly string path = Application.persistentDataPath;
    private static readonly string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\";

    private Dictionary<string, Dictionary<string, int>> data;
    private static readonly string[] parts = new string[] { "Puzzle", "Setting"};

    private void Start()
    {
        data = LoadData();

        //SaveData();
    }

    public static void Load(bool flag = false)
    {
        instance.data = LoadData(flag);
    }

    public static void SetData(string part, string key, int value)
    {
        instance.data[part][key] = value;
    }

    public static bool HasData(string part, string key)
    {
        return instance.data[part].ContainsKey(key);
    }

    public static int GetData(string part, string key, int def = -1)
    {
        if (!instance.data[part].ContainsKey(key)) return def;
        return instance.data[part][key];
    }

    public static void SaveData()
    {
        foreach (string part in parts) { 
            XElement el = new XElement("root", instance.data[part].Select(kv => new XElement(kv.Key, kv.Value)));

            el.Save(path + part + ".xml");
        }
    }

    private static Dictionary<string, Dictionary<string, int>> LoadData(bool flag = false)
    {
        var data = new Dictionary<string, Dictionary<string, int>>();
        foreach (string part in parts)
        {
            string tmp = path + part + ".xml";
            data[part] = new Dictionary<string, int>();

            if (flag) continue;
            if (File.Exists(tmp)) { 
                XElement root = XElement.Load(tmp);
                foreach (var element in root.Elements())
                    data[part].Add(element.Name.LocalName, int.Parse(element.Value));
            }
        }

        return data;
    }
}
