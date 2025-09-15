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

   
   



}
