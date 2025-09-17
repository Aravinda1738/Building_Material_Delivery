using UnityEngine;

[CreateAssetMenu(fileName = "SO_DebugMode", menuName = "Scriptable Objects/SO_DebugMode")]
public class SO_DebugMode : ScriptableObject
{
    public bool isDebugMode = true;   

    public GameObject debugCube;
}
