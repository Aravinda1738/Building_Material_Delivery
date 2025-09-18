using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public static class DebuggingTools
{
    public static void PrintList<T>(string aditionalMessage, List<T> arr)
    {
        string message = string.Join(",",arr);

        Debug.Log(string.Join(aditionalMessage, $" Items [ { message} ]"));
        
    }

    public static void PrintListOfLists<T>(string additionalMessage, List<List<T>> arr)
    {
        string message = $"{additionalMessage}";
        List<string> temp = new List<string>();

        foreach (List<T> list in arr) {
           
            temp.Add($"[{(string.Join(",", list))}]");
           
        }

           
        
        Debug.Log($"{message} items : {string.Join(" , ",temp)} ");
    }

    public static void SpawnDebugObjs(GameObject debugObj, List<Vector3> locations, Quaternion rotation)
    {
        List<GameObject> debugObjs = new List<GameObject>();

        if (locations == null)
        {
            return;
        }

        foreach (Vector3 pos in locations)
        {
            GameObject gameObject = MonoBehaviour.Instantiate(debugObj, pos, rotation);

            debugObjs.Add(gameObject);





        }

    }

    public static void SpawnDebugObjs(GameObject debugObj, List<Vector3> locations, Quaternion rotation,Transform parent)
    {
        List<GameObject> debugObjs = new List<GameObject>();

        if (locations == null)
        {
            return;
        }
        int i = 0;
        foreach (Vector3 pos in locations)
        {
            GameObject gameObject = MonoBehaviour.Instantiate(debugObj, pos, rotation);

            debugObjs.Add(gameObject);
            gameObject.name = $"Debug_OBJ_{i}";
            gameObject.transform.SetParent(parent);



        }

    }


    public static void clearDebugObjs(List<GameObject> debugObjs)
    {
        foreach (GameObject obj in debugObjs)
        {
            MonoBehaviour.DestroyImmediate(obj);
        }
        debugObjs.Clear();



    }

    public static void PrintMessage(string message, DebugMessageType type, object from)
    {
        switch (type)
        {
            case DebugMessageType.INFO:

                Debug.Log($"{message} || From Class - > {from}");
                break;
            case DebugMessageType.WARNING:
                Debug.LogWarning($"{message} || From Class - > {from}");
                break;
            case DebugMessageType.ERROR:
                Debug.LogError($"{message} || From Class - > {from}");
                break;
            default:
                break;
        }



    }

    public static void PrintMessage(string message, object from)
    {
       
            Debug.Log( $"{message} || From Class - > {from}");
       
    }


    public enum DebugMessageType
    {
        INFO,
        WARNING,
        ERROR
    }
}

