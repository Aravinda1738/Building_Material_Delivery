using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    private int DifficultyLevel = 3; // (5,4)-easy,(2,3)-medium,0-hard

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


        baseArr = CustomeShuffle(DifficultyLevel, baseArr, containerData.totalItemsCanHold); // custome shuffle algorathem


        if (debugMode.isDebugMode)
        {
            
            PrintIntList("=== After shuffle===", baseArr);
        }

        if (containers.Count == 0)
        {
            Debug.LogError("containers Array is Empty.");
            return;
        }

        int skip = 0;
        for (int i = 1; i < cells.Count + 1; i++) // split and give to containers
        {


            List<int> temp = new List<int>();


            for (int j = skip; j < containerData.totalItemsCanHold * i; j++)
            {

                temp.Add(baseArr[j]);
                skip++;
            }
            // skip++;
            containers[i - 1].InitialLoad(temp);






            if (debugMode.isDebugMode)
            {
                PrintIntList("Per Vehical Load",temp);
            }
        }




    }


    private List<int> CustomeShuffle(int numberOfMatchingNeighbors, List<int> arr, int totalTypes)
    {
        //arr.Sort((a, b) => Random.Range(-1, 2)); //initial 

       // FisherYatesShuffle(arr);

        int[] typesToSwap = new int[2];
        typesToSwap[0] = Random.Range(0, totalTypes);
        typesToSwap[1] = Random.Range(0, totalTypes);

        List<int> SwapCompletedIndexes = new List<int>();
        int swapId = 0;

        if (debugMode.isDebugMode)
        {
             PrintIntList("Shuffle = ",arr);
        }

        for (int i = 0; i < numberOfMatchingNeighbors; i++)
        {
            int indexOfA = 0;
            int indexOfB = 0;

            for (int k = 0; k < arr.Count; k++)
            {
                // Debug.Log("k: " + k + "/" + arr.Count);

                if (k != arr.Count - 1) //ignore last index
                {

                    if (arr[k] == typesToSwap[swapId] && arr[k] != arr[k + 1])   //ignore already swapped or grouped before swapped indexes
                    {
                        if (k != 0)
                        {
                            if (arr[k] != arr[k - 1])
                            {
                                indexOfA = arr.IndexOf(typesToSwap[0]);  //1




                                foreach (int item in arr)                //2
                                {
                                    if (arr.IndexOf(item) != indexOfA && item == arr[indexOfA])
                                    {
                                        indexOfB = arr.IndexOf(item);

                                    }
                                }

                                arr = Swap(arr, indexOfA, indexOfB);  //3
                            }
                        }
                        else
                        {
                            indexOfA = arr.IndexOf(typesToSwap[0]);



                            foreach (int item in arr)
                            {
                                if (arr.IndexOf(item) != indexOfA && item == arr[indexOfA])
                                {
                                    indexOfB = arr.IndexOf(item);

                                }
                            }

                            arr = Swap(arr, indexOfA, indexOfB);
                        }

                        if (swapId >= 1)
                        {
                            swapId = 0;

                        }
                        else
                        {

                            swapId++;
                        }


                    }
                }

            }
        }
        return arr;
    }




    // TOOLS

    private List<int> Swap(List<int> arr, int indexA, int indexB) //swap a+1 and b
    {
        int tempA;


        tempA = arr[indexA + 1];

        arr[indexA + 1] = arr[indexB];

        arr[indexB] = tempA;


        return arr;


    }

    private void FisherYatesShuffle(List<int> list)
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
