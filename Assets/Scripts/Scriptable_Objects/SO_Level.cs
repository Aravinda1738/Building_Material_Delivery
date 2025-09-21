using UnityEngine;

[CreateAssetMenu(fileName = "SO_Level", menuName = "Scriptable Objects/SO_Level")]
public class SO_Level : ScriptableObject
{
    [Header("Game Data")]
    [SerializeField]
    private int totalVehicalsToSpawn = 5;
    [SerializeField]
    private int TotalMovesAvailable = 5;


    [Tooltip("important point: must not excede totalSpawnPoints always add totalEmptyVehicals+addable Container ")]
    [SerializeField]
    private int totalEmptyVehicals = 2;



    [SerializeField]
    private int numberOfPairsInLevel = 3;
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

    public int GetNumberOfPairs() { return numberOfPairsInLevel; }

    public int GetTotalMovesAvailable() { 
     
        return TotalMovesAvailable;
    
    }


  
}
