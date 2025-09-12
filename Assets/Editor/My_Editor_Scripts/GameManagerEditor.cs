using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameManager gameManager = (GameManager)target;
        if (GUILayout.Button("Generate Parking Spots"))
        {
            gameManager.GenerateParkingSpots();
        }

        if (GUILayout.Button("Clear  Parking Spots"))
        {
            gameManager.ClearParkngSpots();
        }



        
    }
}
