using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DebugMode", menuName = "Scriptable Objects/SO_DebugMode")]
public class SO_DebugMode : ScriptableObject
{
    public bool isDebugMode = true;

    public GameObject debugCube;






    public void PrintList<T>(string aditionalMessage, List<T> arr)
    {
        if (isDebugMode)
        {

            string message = string.Join(",", arr);

            Debug.Log(string.Join(aditionalMessage, $" Items [ {message} ]"));
        }

    }

    public void PrintListOfLists<T>(string additionalMessage, List<List<T>> arr)
    {
        if (isDebugMode)
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

    public void SpawnDebugObjs(GameObject debugObj, List<Vector3> locations, Quaternion rotation)
    {

        if (isDebugMode)
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
    public void SpawnDebugObjs(GameObject debugObj, List<LoadingSpots> locations, Quaternion rotation, Transform parent)
    {
        if (isDebugMode)
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

    public void SpawnDebugObjs(GameObject debugObj, List<Vector3> locations, Quaternion rotation, Transform parent)
    {
        if (isDebugMode)
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


    public void clearDebugObjs(List<GameObject> debugObjs)
    {
        if (isDebugMode)
        {

            foreach (GameObject obj in debugObjs)
            {
                MonoBehaviour.DestroyImmediate(obj);
            }
            debugObjs.Clear();
        }



    }

    public void PrintMessage(string message, DebugMessageType type, object from)
    {
        if (isDebugMode)
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

    public void PrintMessage(string message, object from)
    {
        if (isDebugMode)
        {

            Debug.Log($"{message} || From Class - > {from}");
        }

    }
    public void PrintMessage(string color, string message, object from)
    {
        if (isDebugMode)
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
