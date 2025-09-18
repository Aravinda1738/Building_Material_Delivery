using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private SO_Container containerData;
    [SerializeField]
    private SO_DebugMode debugMode;
    [SerializeField]
    private SO_Item itemData;

    [SerializeField]
    private float gapBetweenItems = 2;
    [SerializeField]
    private GameObject sp;

    private float moveSpeed = 0.005f;

    private Vector3 moveToPos;

    public bool isMovingOut = false;

    private List<Vector3> loadingSpots = new List<Vector3>();


    [Tooltip("you can add only 4 elements")]
    private List<GameObject> items = new List<GameObject>();

    public bool spawnDebugobj = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void Start()
    {
        moveToPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 500);

    }

    private void FixedUpdate()
    {
        //on game win
        if (isMovingOut)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, moveToPos.z, moveSpeed * Time.deltaTime));
        }

    }


    public void InitialLoad(List<int> ids)
    {
        if (debugMode.isDebugMode)
         DebuggingTools.PrintMessage(" Initial Load ", this);   

        GenerateLoadingSpots();
        int i = 0;
        foreach (Vector3 item in loadingSpots)
        {
            if (i>itemData.GetitemTypesCount())
            {
                DebuggingTools.PrintMessage(" id > item type ", DebuggingTools.DebugMessageType.ERROR, this);
            }

            GameObject temp = Instantiate(itemData.GetItemType(ids[i]), item, itemData.GetItemType(ids[i]).transform.rotation);
            // GameObject temp = Instantiate(debugMode.debugCube, item, sp.transform.rotation);
            temp.name = string.Join("Item_", i);
            // temp.transform.localScale = new Vector3(.5f, .5f, .5f);
            temp.transform.SetParent(sp.transform);
            i++;

        }
    }


    public void PickAction()
    {

    }


    public void Load()
    {

    }

    public void Unload()
    {

    }

    public void MoveOut()
    {
        isMovingOut = true;




    }


   public void GenerateLoadingSpots()
    {
        loadingSpots.Clear();

        for (int i = 0; i < containerData.totalItemsCanHold; i++)
        {
            loadingSpots.Add(new Vector3(sp.transform.position.x,sp.transform.position.y ,sp.transform.position.z + ((i+1) * gapBetweenItems)));
        }
     
        if (debugMode.isDebugMode&spawnDebugobj)
        {
            DebuggingTools.SpawnDebugObjs(debugMode.debugCube, loadingSpots, sp.transform.rotation,sp.transform);
           
        }

    }



}
