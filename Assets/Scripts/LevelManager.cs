using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;
using static UnityEditor.Progress;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startPos;
    private List<Vector3> cells = new List<Vector3>();

    [SerializeField]
    private SO_Level_Manager levelManagerData;
    [SerializeField]
    private SO_Container containerData;

    private List<Container> containers = new List<Container>();
    [SerializeField]
    private SO_Item itemData;
    [SerializeField]
    private int DifficultyLevel = 3;// (5,4)-easy,(2,3)-medium,0-hard

    //Debug Variables
    [SerializeField]
    private SO_DebugMode debugMode;
    [SerializeField]
    private bool spawnDebugobj;


    private List<GameObject> debugObjs = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        //if (levelManagerData == null)
        //{
        //    Debug.LogError("Level Manager Data is not assigned in the inspector.");
        //    return;
        //}
        // GenerateParkingSpots();

    }


    void Start()
    {
        if (debugMode.isDebugMode)
            clearDebugObjs();

        GenerateParkingSpots();
        //SpawnContainers();

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

            Debug.Log("Total Parking Spots: " + cells.Count);

            if (spawnDebugobj)
            {
                SpawnDebugObjs();
            }
        }
    }

    public void ClearParkngSpots()
    {
        if (debugMode.isDebugMode)
        {
            clearDebugObjs();
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
                Debug.LogError("does not have a Container component attached.");
            }
        }
    }




    private void DoShuffle()
    {
        if (cells.Count == 0)
        {
            Debug.LogError("Parking Spots are not generated.");
            return;

        }

        int totalItems = cells.Count * containerData.totalItemsCanHold;

        List<int> baseArr = new List<int>();

        for (int i = 0; i < levelManagerData.totalSpawnPoints; i++) // generate itemId in array // unshuffed
        {
            for (int j = 0; j < containerData.totalItemsCanHold; j++)
            {
                baseArr.Add(j);
            }


        }

        if (debugMode.isDebugMode)
        {

            PrintIntList("=== Before shuffle===", baseArr);
        }


        List<List<int>> setsOfItmsPerContainer = new List<List<int>>();

        setsOfItmsPerContainer = CustomeShuffle2(DifficultyLevel, baseArr, containerData.totalItemsCanHold); // custome shuffle algorathem

        int emptyContainers =0;
        if (DifficultyLevel>2)
        {
            emptyContainers = 2;
        }
        else
        {
            emptyContainers = 1;
        }

        for (int i = 0; i < cells.Count-emptyContainers; i++)
        {
            containers[i].InitialLoad(setsOfItmsPerContainer[i]);
        }


    }


  


    private List<List<int>> CustomeShuffle2(int numberOfMatchingNeighbors, List<int> arr, int totalTypes)
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

         result = SwapItemsFromArraysV4(FisherYatesShuffle(arr), numberOfMatchingNeighbors);


        if (debugMode.isDebugMode)
        {
            PrintListOfLists("===  shuffle v4 ===", result);

        }
        return result;

       
    }

    // TOOLS
    class ItemsSet
    {
        public List<int> items;
        public bool isComplete = false;

        public int matchingPairsCount;

    }

    private List<List<int>> SwapItemsFromArraysV4(List<int> arr, int requiredNoOfPairs) //swap V4
    {
        List<int> temp = new List<int>();
        int splitAmount = arr.Count;

        foreach (var item in arr)
        {
            
                temp.Add(item);
            
        }

        int pairs = 0;





        PrintIntList("1 temp", temp);

        for (int k = 0; k < temp.Count; k++)// sort pairs in temp
        {
            if (k + 2 < temp.Count)
            {
                if (temp[k] == temp[k + 1]&& temp[k] != temp[k + 2])
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
                            arr.Clear();


                            Debug.LogWarning("temp count = " + temp.Count + " Pairs = " + pairs);

                            return splitPairs(temp, splitAmount);



                        }
                        int t = temp[k + 1];
                        temp[k + 1] = temp[j];
                        temp[j] = t;

                        pairs++;
                    }

                }


            }


        }

        arr.Clear();




        Debug.LogWarning("temp count = " + temp.Count + " Pairs = " + pairs);



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

    //Debug Functions
    private void PrintIntList(string aditionalMessage, List<int> arr)
    {
        string message = "ARRAY - ";
        foreach (int item in arr)
        {
            message += item + ", ";
        }
        Debug.Log(aditionalMessage + " Item : " + message);
    }

    private void PrintListOfLists(string additionalMessage, List<List<int>> arr)
    {
        string message = additionalMessage + " [";
        for (int i = 0; i < arr.Count; i++)
        {

            for (int j = 0; j < arr[i].Count; j++)
            {
                message += arr[i][j] + " , ";
            }
            message += "] [";
        }
        Debug.Log(message);
    }

    private void PrintItemSet(string aditionalMessage, List<ItemsSet> arr)
    {
        string message = "SET - [";
        foreach (var item in arr)
        {
            foreach (var item1 in item.items)
            {
                message += item1 + ", ";

            }
            message += "] Pairs = " + item.matchingPairsCount + " | ";
        }
        Debug.Log(aditionalMessage + " Item : " + message);
    }

    private void PrintIntArray(string aditionalMessage, int[] arr)
    {
        string message = "ARRAY - ";
        foreach (int item in arr)
        {
            message += item + ", ";
        }
        Debug.Log(aditionalMessage + " Item : " + message);
    }
    public void SpawnDebugObjs()
    {
        if (cells == null)
        {
            return;
        }

        foreach (Vector3 pos in cells)//loop
        {
            GameObject gameObject = Instantiate(debugMode.debugCube, pos, levelManagerData.Vehical.transform.rotation);

            debugObjs.Add(gameObject);





        }

    }
    public void clearDebugObjs()
    {
        foreach (GameObject obj in debugObjs)
        {
            DestroyImmediate(obj);
        }
        debugObjs.Clear();



    }

}
