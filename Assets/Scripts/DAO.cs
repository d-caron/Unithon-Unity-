using UnityEngine;
using static S_World;

[System.Serializable]
public class DAO {
    public string type;
    public string action;
    public string[] characters;
    public S_World world;

    public string Serialize () {
        return JsonUtility.ToJson (this, true);
    }

    public DAO Deserialize (string json) {
        JsonUtility.FromJsonOverwrite (json, this);
        return this;
    }

    public static void main () {
        Debug.Log ("Hello World");
    }
}