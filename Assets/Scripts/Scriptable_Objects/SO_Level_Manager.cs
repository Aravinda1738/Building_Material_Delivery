using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Level_Manager", menuName = "Scriptable Objects/SO_Level_Manager")]
public class SO_Level_Manager : ScriptableObject
{
    [Header("Layout")]
    public int rows = 3;
    public int columns = 3;
    public int totalSpawnPoints = 5;


    [Header("Adjustments")]
    public float spaceBetweenColumns = 5f;
    public float spaceBetweenRows = 8f;

    public float rowOffsetFromSP = -5.5f;
    public float columnOffsetFromSP = -5.5f;

    public float SpawnHeight = 1.195f;

    [Header("Spawn Object")]
    public GameObject Vehical;


    [Header("Game Data")]
    public int difficultyLevel = 3;
    public bool useDificultyForEmptyContainers = true;
    [HideInInspector]
    public int totalTypesInGame;

    [Tooltip("important point: must not excede totalSpawnPoints ")]
    public int emptyContainers = 2;


    private void OnValidate()
    {
        if(emptyContainers> totalSpawnPoints)
        {
            emptyContainers= totalSpawnPoints;
        }
    }
}
