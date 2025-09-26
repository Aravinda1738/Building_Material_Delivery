using System.Collections.Generic;
using UnityEngine;

public static class DebuggingTools
{
    private static SO_DebugMode debugMode;




    public static void PrintList<T>(string aditionalMessage, List<T> arr)
    {
        if (debugMode.isDebugMode)
        {

            string message = string.Join(",", arr);

            Debug.Log(string.Join(aditionalMessage, $" Items [ {message} ]"));
        }

    }

    public static void PrintListOfLists<T>(string additionalMessage, List<List<T>> arr)
    {
        if (debugMode.isDebugMode)
        {
            string message = $"{additionalMessage}";
            List<string> temp = new List<string>();

            foreach (List<T> list in arr)
            {

                temp.Add($"[{(string.Join(",", list))}]");

            }



            Debug.Log($"{message} items : {string.Join(" , ", temp)} ");

        }

    }

    public static  void SpawnDebugObjs(GameObject debugObj, List<Vector3> locations, Quaternion rotation)
    {

        if (debugMode.isDebugMode)
        {
            if (locations == null)
            {
                Debug.LogError($"{locations} is empty");
                return;
            }

            foreach (Vector3 pos in locations)
            {

                MonoBehaviour.Instantiate(debugObj, pos, rotation);

            }

        }
    }
    public static  void SpawnDebugObjs(GameObject debugObj, List<LoadingSpots> locations, Quaternion rotation, Transform parent)
    {
        if (debugMode.isDebugMode)
        {

            if (locations == null || locations.Count == 0)
            {
                Debug.LogError($"{locations} is empty");
                return;
            }

            foreach (var item in locations)
            {

                GameObject gameObject = MonoBehaviour.Instantiate(debugObj, item.spot, rotation);

                gameObject.transform.SetParent(parent);
            }

        }
    }

    public static void SpawnDebugObjs(GameObject debugObj, List<Vector3> locations, Quaternion rotation, Transform parent)
    {
        if (debugMode.isDebugMode)
        {
            if (locations == null)
            {
                Debug.LogError($"{locations} is empty");
                return;
            }
            int i = 0;
            foreach (Vector3 pos in locations)
            {
                GameObject gameObject = MonoBehaviour.Instantiate(debugObj, pos, rotation);

                gameObject.name = $"Debug_OBJ_{i}";
                gameObject.transform.SetParent(parent);



            }

        }
    }


    public static void clearDebugObjs(List<GameObject> debugObjs)
    {
        if (debugMode.isDebugMode)
        {

            foreach (GameObject obj in debugObjs)
            {
                MonoBehaviour.DestroyImmediate(obj);
            }
            debugObjs.Clear();
        }



    }

    public static void PrintMessage(string message, DebugMessageType type, object from)
    {
        if (debugMode.isDebugMode)
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


    }

    public static void PrintMessage(string message, object from)
    {
        if (debugMode.isDebugMode)
        {

            Debug.Log($"{message} || From Class - > {from}");
        }

    }
    public static void PrintMessage(string color, string message, object from)
    {
        if (debugMode.isDebugMode)
        {

            Debug.Log($"<color={color}>{message} || From Class - > {from}</color>");
        }

    }


    public enum DebugMessageType
    {
        INFO,
        WARNING,
        ERROR
    }




}

