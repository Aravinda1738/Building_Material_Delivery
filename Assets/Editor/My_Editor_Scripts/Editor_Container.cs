using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Container))]
public class Editor_Container : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Container Container = (Container)target;
        if (GUILayout.Button("Generate Loading Spots"))
        {
            Container.GenerateLoadingSpots();
        }

        //if (GUILayout.Button("Clear  Parking Spots"))
        //{
        //    Container.ClearParkngSpots();
        //}




    }
}
