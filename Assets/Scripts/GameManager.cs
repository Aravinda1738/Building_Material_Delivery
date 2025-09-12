using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startPos;
    private List<Vector3> cells;

    public SO_Level_Manager levelManagerData;



    //Debug Variables
    public bool debugMode = true;



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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Touched object: " + hit.transform.name);
                }

            }
        }
    }



    private void CheckPickableItems()
    {
    }

    private void CheckCanDropPickedItems()
    {
    }

    public void GenerateParkingSpots()
    {
        int y = 0;
        cells = new List<Vector3>();
        for (int i = 0; i < levelManagerData.maxNumberOfParkingSpots; i++)
        {
            if (i%3==0)
            {
                //shift to next row
                y++;
            }
            Vector3 newCell = new Vector3(startPos.transform.position.x + (i * levelManagerData.spaceBetween), y+startPos.transform.position.y, startPos.transform.position.z);
                cells.Add(newCell);
                if (debugMode)
                {

                    SpawnDebugObjs();



                }
            
        }
       
      
    }


    public void ClearParkngSpots()
    {
        if (debugMode)
        {
            clearDebugObjs();
        }
        cells.Clear();
    }



    //Debug Functions
    public void SpawnDebugObjs()
    {
        if (cells == null)
        {
            return;
        }

        foreach (Vector3 pos in cells)
        {
            Instantiate(levelManagerData.Vehical, pos, levelManagerData.Vehical.transform.rotation);

        }

    }
    public void clearDebugObjs()
    {


    }


}
