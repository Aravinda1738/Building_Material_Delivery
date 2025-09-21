using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Level_Manager", menuName = "Scriptable Objects/SO_Level_Manager")]
public class SO_Level_Manager : ScriptableObject
{
    [Header("Layout")]
    public int rows = 3;
    public int columns = 3;


    [Header("Layout Adjustments")]
    public float spaceBetweenColumns = 5f;
    public float spaceBetweenRows = 8f;

    public float rowOffsetFromSP = -5.5f;
    public float columnOffsetFromSP = -5.5f;

    public float spawnHeight = 1.195f;

    [Header("Spawn Object")]
    public GameObject Vehical;

    [Tooltip("After last level which level should player go to ")]
    [SerializeField]
    private int fallBackLevel = 3;


    //Hidden
    [SerializeField]
    private int playingLevel=1;
    
    private int totalTypesInGame;

    
   // public int emptyContainers = 2;

    [Header("Game Levels")]
    [Tooltip("Index 0 = Level 1 ,Index 1 = Level 2... ")]
    [SerializeField]
    private List<SO_Level> levels = new List<SO_Level>();


    [Header("Debug")]
    [SerializeField]
    private bool AlwaysStartFromLevelOne=true;


    private void OnEnable()
    {
        

            if (AlwaysStartFromLevelOne)
            {
                playingLevel = 1;
            }
        
    }







    public SO_Level GetLevel(int level)
    {
        return levels[level];
    }
    public SO_Level GetCurrentLevelData()
    {
        if (levels.Count == 0)
        {
            DebuggingTools.PrintMessage("Levels List is Empty!!! ",DebuggingTools.DebugMessageType.ERROR,this) ; 
            return null;
        }
        return levels[playingLevel-1];
    }
    
    public int GetCurrentLevel()
    {
        return playingLevel;
    }


    public void IncrementCurrentLevel()
    {
        playingLevel++;
        if (playingLevel > levels.Count) {

            playingLevel = fallBackLevel;

            
        }
        //if (!AlwaysStartFromLevelOne)
        //{
        //    lastPlayedLevel = playingLevel;
        //}
    }

    public int GetTotalTypesInGame()
    {
        return totalTypesInGame;
    }
    public void SetTotalTypesInGame(int value)
    {
         totalTypesInGame=value;
    }
}
