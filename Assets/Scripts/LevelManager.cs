using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField]
    private GameObject startPos;
    [SerializeField]
    private SO_Level_Manager levelManagerData;
    [SerializeField]
    private SO_Container containerData;
    [SerializeField]
    private SO_Item itemData;
    [SerializeField]
    private SO_TransactionEventChannel TransactionEventChannel;
    [SerializeField]
    private SO_UIChannel uIChannel;

    [SerializeField]
    private float clearlevelDelay = 5;


    private List<Vector3> cells = new List<Vector3>();
    private List<Container> containers = new List<Container>();


    //Debug Variables
    [Header("Debug")]
    [SerializeField]
    private SO_DebugMode debugMode;
    [SerializeField]
    private bool canSpawnDebugobj;


    private List<GameObject> debugObjs = new List<GameObject>();

    private GameObject ExtraContainer;

    private void OnEnable()
    {
        if (TransactionEventChannel != null)
        {
            TransactionEventChannel.onWin += ClearLevel;
            TransactionEventChannel.onGameOver += QuitLevel;
        }
       

        if (uIChannel != null)
        {
            uIChannel.onStartGame += StartLevel;
            uIChannel.onNextLevel += PrepairLevel;
            uIChannel.onBackToHome += QuitLevel;
            uIChannel.onAddExtraContainer += AddExtraContainer;
        }
        




    }


    private void OnDisable()
    {
        if (TransactionEventChannel != null)
        {
            TransactionEventChannel.onWin -= ClearLevel;
            TransactionEventChannel.onGameOver -= QuitLevel;
        }

        if (uIChannel != null)
        {
            uIChannel.onStartGame -= StartLevel;
            uIChannel.onNextLevel -= PrepairLevel;
            uIChannel.onBackToHome -= QuitLevel;
            uIChannel.onAddExtraContainer -= AddExtraContainer;



        }
    }

    private void Awake()
    {
        if (debugMode.isDebugMode)
            debugMode.clearDebugObjs(debugObjs);

    }


    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartLevel()
    {

        uIChannel.OnUpdateLevelText(levelManagerData.GetCurrentLevel(),levelManagerData.GetCurrentLevelData().GetTotalMovesAvailable());

        GenerateParkingSpots();

        SpawnContainers();

        DoShuffle();
    }



    public void ClearLevel()
    {
        ClearLevelAfterDelay();
    }
    private IEnumerator ClearLevelAfterDelay()
    {
        debugMode.PrintMessage("green", "CLEAR LEVEL CALLED ", this);
        yield return new WaitForSeconds(clearlevelDelay);
        ClearContainers();
        ClearParkngSpots();
        //PrepairNextLevel();
    }


    private void PrepairLevel(bool isLevelComplete)
    {
        if (isLevelComplete)
        {
         levelManagerData.IncrementCurrentLevel();

        }
        //Level complete popup
        //after click next load level or back to home
        StartLevel();
    }

    private void QuitLevel()
    {
        ClearContainers();
        ClearParkngSpots();
    }

    private void AddExtraContainer()
    {
        ExtraContainer.SetActive(true);
    }


    public void GenerateParkingSpots()
    {

        ClearParkngSpots();
        float z = startPos.transform.position.z - levelManagerData.rowOffsetFromSP;
        float x = startPos.transform.position.x - levelManagerData.columnOffsetFromSP;
        for (int i = 0; i < levelManagerData.rows; i++)
        {

            z += levelManagerData.spaceBetweenRows;

            for (int j = 1; j < (levelManagerData.columns + 1); j++)
            {


                x += levelManagerData.spaceBetweenColumns;

                if (cells.Count < levelManagerData.GetCurrentLevelData().GetTotalVehicalsToSpawn())
                {


                    cells.Add(new Vector3(x, levelManagerData.spawnHeight, startPos.transform.position.z - z));
                }



            }
            x = (startPos.transform.position.x - levelManagerData.columnOffsetFromSP);


        }
       
            debugMode.PrintMessage($"Total Parking Spots:  {cells.Count} ", this);

            if (canSpawnDebugobj)
            {
                debugMode.SpawnDebugObjs(debugMode.debugCube, cells, transform.rotation);
            }
        
    }

    public void ClearParkngSpots()
    {
        
            debugMode.clearDebugObjs(debugObjs);
        

        cells.Clear();
    }


    private void SpawnContainers()
    {
        ClearContainers();
        int i = 0;
        foreach (Vector3 pos in cells)
        {
            GameObject containerObj = Instantiate(levelManagerData.Vehical, pos, levelManagerData.Vehical.transform.rotation);
            Container container = containerObj.GetComponent<Container>();
            if (container != null)
            {
                container.SetId(i);
                if ((cells.Count-1) == i)
                {
                    ExtraContainer=container.gameObject;
                     container.gameObject.SetActive(false);
                }
                containers.Add(container);

                i++;
            }
            else
            {
                debugMode.PrintMessage("does not have a Container component attached.", SO_DebugMode.DebugMessageType.ERROR, this);


            }
        }
    }

    public void ClearContainers()
    {
       
            debugMode.clearDebugObjs(debugObjs);
        
        foreach (var item in containers)
        {
            Destroy(item.gameObject);

        }
        containers.Clear();
    }

    

    private void DoShuffle()
    {
        if (cells.Count == 0)
        {

            debugMode.PrintMessage("Parking Spots are not generated/Empty.", SO_DebugMode.DebugMessageType.ERROR, this);
            return;

        }

        int totalItems = cells.Count * containerData.totalItemsCanHold;

        List<int> baseArr = new List<int>();


        int emptyContainers = levelManagerData.GetCurrentLevelData().GetTotalEmptyVehicals();

        int typesToSpawn = 0;
        int availableSpots = levelManagerData.GetCurrentLevelData().GetTotalVehicalsToSpawn() - emptyContainers;
        for (int i = 0; i < availableSpots; i++, typesToSpawn++) // generate itemId in array // unshuffed
        {
            if (typesToSpawn > itemData.GetitemTypesCount())
            {
                typesToSpawn = 0;
            }
            for (int j = 0; j < containerData.totalItemsCanHold; j++)
            {


                baseArr.Add(typesToSpawn);
            }


        }

      
            debugMode.PrintList("=== Before shuffle===", baseArr);
        

        List<List<int>> setsOfItmsPerContainer = CustomeShuffle2(levelManagerData.GetCurrentLevelData().GetNumberOfPairs(), baseArr, containerData.totalItemsCanHold, containerData.totalItemsCanHold); // custome shuffle algorathem
        levelManagerData.SetTotalTypesInGame(setsOfItmsPerContainer.Count);
        
            debugMode.PrintMessage($"Total Parking Spots:  {cells.Count} ", this);
        


        for (int i = 0; i < cells.Count; i++)
        {
            if (i >= cells.Count - emptyContainers)
            {
                containers[i].GenerateLoadingSpots();
            }
            else
            {
                containers[i].InitialLoad(setsOfItmsPerContainer[i]);

            }

        }


    }





    private List<List<int>> CustomeShuffle2(int numberOfMatchingNeighbors, List<int> arr, int totalTypes, int splitAmount)
    {


        List<List<int>> result = new List<List<int>>();

        int types = cells.Count - 1;
        if (itemData.GetitemTypesCount() < cells.Count - 1)
        {
            types = itemData.GetitemTypesCount();
        }
        else
        {
            types = cells.Count - 1;
        }

        result = SwapItemsFromArraysV4(FisherYatesShuffle(arr), numberOfMatchingNeighbors, splitAmount);


         debugMode.PrintListOfLists("===  shuffle v4 ===", result);

        
        return result;


    }









    private List<List<int>> SwapItemsFromArraysV4(List<int> arr, int requiredNoOfPairs, int splitAmount) //swap V4
    {
        List<int> temp = new List<int>();

        foreach (var item in arr)
        {
            temp.Add(item);
        }

        int pairs = 0;

        debugMode.PrintList("temp", temp);

        int lastPairIndex = -1; // Track the last index included in pairs

        for (int k = 0; k < temp.Count; k++) // sort pairs in temp
        {
            if (k + 2 < temp.Count)
            {
                if (temp[k] == temp[k + 1] && temp[k] != temp[k + 2])
                {
                    k += 2;
                    pairs++;
                    lastPairIndex = k - 1; // last paired item index
                    continue;
                }
            }

            for (int j = 0; j < temp.Count; j++)
            {
                if (k + 1 < temp.Count)
                {
                    if (temp[k] == temp[j] && j != k)
                    {
                        if (pairs == requiredNoOfPairs)
                        {
                            if (debugMode.isDebugMode)
                                debugMode.PrintMessage($"temp count = {temp.Count} Pairs = {pairs}", this);

                            goto AfterPairs; // Break out to shuffle leftovers
                        }

                        if (k + 3 < temp.Count && k - 1 >= 0)
                        {
                            if (temp[k] == temp[k + 1] && temp[k] == temp[k + 2] && temp[k] == temp[k + 3])
                            {
                                int o = temp[k + 1];
                                int randomIndex = k - 1;

                                temp[k + 1] = temp[randomIndex];
                                temp[randomIndex] = o;
                            }
                            else
                            {
                                int t = temp[k + 1];
                                temp[k + 1] = temp[j];
                                temp[j] = t;
                            }
                        }

                        pairs++;
                        lastPairIndex = Math.Max(lastPairIndex, k + 1);
                    }
                }
            }
        }

    AfterPairs:

        // Shuffle remaining items after lastPairIndex
        if (lastPairIndex < temp.Count - 1)
        {
            FisherYatesShuffle(temp, lastPairIndex + 1);
        }

        // Prevent quadruple consecutive identical items
        for (int k = 0; k < temp.Count - 3; k++)
        {
            if (temp[k] == temp[k + 1] && temp[k] == temp[k + 2] && temp[k] == temp[k + 3])
            {
                int swapIndex = -1;
                for (int m = k + 4; m < temp.Count; m++)
                {
                    if (temp[m] != temp[k])
                    {
                        swapIndex = m;
                        break;
                    }
                }
                if (swapIndex != -1)
                {
                    int t = temp[k + 3];
                    temp[k + 3] = temp[swapIndex];
                    temp[swapIndex] = t;
                }
            }
        }

        arr.Clear();

        
            debugMode.PrintMessage($"temp count = {temp.Count} Pairs = {pairs}", this);

        return splitPairs(temp, splitAmount);
    }






    private List<List<int>> splitPairs(List<int> temp, int splitAmount)
    {

        var result = new List<List<int>>();
        //split again and return
        for (int i = 0; i < temp.Count; i += splitAmount)
        {
            int count = Mathf.Min(splitAmount, temp.Count - i);


            result.Add(temp.GetRange(i, count));
        }
        
            debugMode.PrintMessage($"Total Parking Spots:  {cells.Count} ", this);
        return result;
    }
    private List<int> FisherYatesShuffle(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    private static void FisherYatesShuffle<T>(List<T> list, int startIndex)
    {
        System.Random rng = new System.Random();
        for (int i = list.Count - 1; i > startIndex; i--)
        {
            int swapIndex = rng.Next(startIndex, i + 1);
            T temp = list[i];
            list[i] = list[swapIndex];
            list[swapIndex] = temp;
        }
    }
   


    



    
   

}
