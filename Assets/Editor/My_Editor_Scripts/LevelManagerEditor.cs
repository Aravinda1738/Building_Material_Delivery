using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelManager levelManager = (LevelManager)target;
        if (GUILayout.Button("Generate Parking Spots"))
        {
            levelManager.GenerateParkingSpots();
        }

        if (GUILayout.Button("Clear  Parking Spots"))
        {
            levelManager.ClearParkngSpots();
        }

        if (GUILayout.Button("Restart Level"))
        {
            levelManager.RestartLevel();
        }



        
    }
}
