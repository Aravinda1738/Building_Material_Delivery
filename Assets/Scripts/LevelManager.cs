using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;
using static UnityEditor.Progress;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private GameObject startPos;
    [SerializeField]
    private SO_Level_Manager levelManagerData;
    [SerializeField]
    private SO_Container containerData;
    [SerializeField]
    private SO_Item itemData;
    [SerializeField]
    private int DifficultyLevel = 3;// (5,4)-easy,(2,3)-medium,0-hard


    private List<Vector3> cells = new List<Vector3>();
    private List<Container> containers = new List<Container>();

   

    //Debug Variables
    [SerializeField]
    private SO_DebugMode debugMode;
    [SerializeField]
    private bool canSpawnDebugobj;


    private List<GameObject> debugObjs = new List<GameObject>();



    private void Awake()
    {
        if (debugMode.isDebugMode)
            DebuggingTools.clearDebugObjs(debugObjs);

    }


    void Start()
    {

        GenerateParkingSpots();

        SpawnContainers();

        DoShuffle();
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void CheckPickableItems()
    {
    }

    private void CheckCanDropPickedItems()
    {
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

                if (cells.Count < levelManagerData.totalSpawnPoints)
                {


                    cells.Add(new Vector3(x, levelManagerData.SpawnHeight, startPos.transform.position.z - z));
                }



            }
            x = (startPos.transform.position.x - levelManagerData.columnOffsetFromSP);


        }
        if (debugMode.isDebugMode)
        {


            DebuggingTools.PrintMessage($"Total Parking Spots:  {cells.Count} ", this);

            if (canSpawnDebugobj)
            {
                DebuggingTools.SpawnDebugObjs(debugMode.debugCube, cells, transform.rotation);
            }
        }
    }

    public void ClearParkngSpots()
    {
        if (debugMode.isDebugMode)
        {
            DebuggingTools.clearDebugObjs(debugObjs);
        }
        cells.Clear();
    }


    private void SpawnContainers()
    {
        foreach (Vector3 pos in cells)
        {
            GameObject containerObj = Instantiate(levelManagerData.Vehical, pos, levelManagerData.Vehical.transform.rotation);
            Container container = containerObj.GetComponent<Container>();
            if (container != null)
            {
                containers.Add(container);
            }
            else
            {
                DebuggingTools.PrintMessage("does not have a Container component attached.", DebuggingTools.DebugMessageType.ERROR, this);


            }
        }
    }




    private void DoShuffle()
    {
        if (cells.Count == 0)
        {

            DebuggingTools.PrintMessage("Parking Spots are not generated/Empty.", DebuggingTools.DebugMessageType.ERROR, this);
            return;

        }

        int totalItems = cells.Count * containerData.totalItemsCanHold;

        List<int> baseArr = new List<int>();


        int emptyContainers = 0;

        switch (DifficultyLevel)
        {
            case 1:
                emptyContainers = 1;
                break;
            case 2:
                emptyContainers = 1;
                break;
            case 3:
                emptyContainers = 2;
                break;
            case 4:
                emptyContainers = 2;
                break;
            case 5:
                emptyContainers = 3;
                break;
            default:
                emptyContainers = 2;
                break;
        }
        int typesToSpawn = 0;
        int availableSpots = levelManagerData.totalSpawnPoints - emptyContainers;
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

        if (debugMode.isDebugMode)
        {

            DebuggingTools.PrintList("=== Before shuffle===", baseArr);
        }

        List<List<int>> setsOfItmsPerContainer = CustomeShuffle2(DifficultyLevel, baseArr, containerData.totalItemsCanHold, containerData.totalItemsCanHold); // custome shuffle algorathem

        if (debugMode.isDebugMode)
        {

            DebuggingTools.PrintMessage($"Total Parking Spots:  {cells.Count} ", this);
        }


        for (int i = 0; i < cells.Count - emptyContainers; i++)
        {
            containers[i].InitialLoad(setsOfItmsPerContainer[i]);
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


        if (debugMode.isDebugMode)
        {
            DebuggingTools.PrintListOfLists("===  shuffle v4 ===", result);

        }
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





        DebuggingTools.PrintList("temp", temp);

        for (int k = 0; k < temp.Count; k++)// sort pairs in temp
        {
            if (k + 2 < temp.Count)
            {
                if (temp[k] == temp[k + 1] && temp[k] != temp[k + 2])
                {
                    k += 2;
                    pairs++;
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
                                DebuggingTools.PrintMessage($"temp count = {temp.Count} Pairs = {pairs}", this);

                            return splitPairs(temp, splitAmount);



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
                    }

                }


            }


        }

        arr.Clear();



        if (debugMode.isDebugMode)
            DebuggingTools.PrintMessage($"temp count = {temp.Count} Pairs = {pairs}", this);



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
        if (debugMode.isDebugMode)
            DebuggingTools.PrintMessage($"Total Parking Spots:  {cells.Count} ", this);
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

    private List<List<int>> SplitList(List<int> source, int splitAmount)
    {
        var result = new List<List<int>>();
        for (int i = 0; i < source.Count; i += splitAmount)
        {
            int count = Mathf.Min(splitAmount, source.Count - i);
            result.Add(source.GetRange(i, count));
        }
        return result;
    }








}
