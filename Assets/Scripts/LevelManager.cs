using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startPos;
    private List<Vector3> cells = new List<Vector3>();

    [SerializeField]
    private SO_Level_Manager levelManagerData;
    private SO_Container containerData;

    private List<Container> containers = new List<Container>();


    //Debug Variables
    [SerializeField]
    private SO_DebugMode debugMode;
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

        // GenerateParkingSpots();
        // SpawnContainers();

        Shuffle();
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
            SpawnDebugObjs();



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




    private void Shuffle()
    {
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
            foreach (int id in baseArr)
            {
                Debug.Log("Item ID: " + id);
            }
        }



        baseArr.Sort((a, b) => Random.Range(-1, 2)); // temp shuffle algorathem 



        for (int i = 0; i < totalItems; i++) // 
        {
            int[] temp = { };

            for (int j = 0; j < containerData.totalItemsCanHold; j++)
            {
                temp[j] = baseArr[i];
            }

            containers[i].InitialLoad(temp);

        }




    }


    //Debug Functions
    public void SpawnDebugObjs()
    {
        if (cells == null)
        {
            return;
        }

        foreach (Vector3 pos in cells)//loop
        {
            GameObject gameObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), pos, levelManagerData.Vehical.transform.rotation);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
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
