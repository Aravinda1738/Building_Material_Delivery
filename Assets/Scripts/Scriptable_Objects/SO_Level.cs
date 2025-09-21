using UnityEngine;

[CreateAssetMenu(fileName = "SO_Level", menuName = "Scriptable Objects/SO_Level")]
public class SO_Level : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField]
    private int totalVehicalsToSpawn = 5;


    [Tooltip("important point: must not excede totalSpawnPoints ")]
    [SerializeField]
    private int totalEmptyVehicals = 2;



    [SerializeField]
    private int difficultyLevel = 3;
    [SerializeField]
  //  private bool useDificultyForEmptyContainers = true;

    private void OnValidate()
    {
        if (totalEmptyVehicals > totalVehicalsToSpawn)
        {
            DebuggingTools.PrintMessage("important point: must not excede totalVehicalsToSpawn",DebuggingTools.DebugMessageType.WARNING, this);
            totalEmptyVehicals = 2;
        }
    }


    public int GetTotalVehicalsToSpawn() { return totalVehicalsToSpawn; }
    public int GetTotalEmptyVehicals() { return totalEmptyVehicals; }

    public int GetDifficultyLevel() { return difficultyLevel; }

  
}
