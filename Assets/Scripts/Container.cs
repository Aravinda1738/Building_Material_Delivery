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

    private int matchingCount = 0;

    public static bool hasSelectedItem = false;

    public string test = "tooo";

    // [Tooltip("you can add only 4 elements")]
    private List<DeleveryItem> items = new List<DeleveryItem>();

    private List<GameObject> pickedGroup = new List<GameObject>();

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
            if (i > itemData.GetitemTypesCount())
            {
                DebuggingTools.PrintMessage(" id > item type ", DebuggingTools.DebugMessageType.ERROR, this);
            }

            GameObject temp = Instantiate(itemData.GetItemType(ids[i]), item, itemData.GetItemType(ids[i]).transform.rotation);
            // GameObject temp = Instantiate(debugMode.debugCube, item, sp.transform.rotation);
            temp.name = string.Join("Item_", i);
            // temp.transform.localScale = new Vector3(.5f, .5f, .5f);
            temp.transform.SetParent(sp.transform);
            items.Add(temp.GetComponent<DeleveryItem>());
            i++;

        }
    }


    public void PickAction()
    {


        if (hasSelectedItem)
        {
            //check if unload is possable

            if (items.Count == 0)
            {
                Unload();

            }

            //if empty -> DROP
            //if picked item & top item of the 2nd cotainer are same & there is  enough space-> DROP
            //if 2nd cotainer is not empty and the top item not same -> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP
            //if 2nd cotainer has same type of item on top but its full or dont have enough space-> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP

        }
        else
        {
            // pickup the top bunch
            if (items.Count != 0)
            {

                SearchValidItems();

            }

        }




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

    public void SearchValidItems()
    {
        int temp = 0;
        DebuggingTools.PrintMessage("GLOW CALL ", this);

        for (int i = items.Count - 2; i > 0; i--)
        {

            items[items.Count - 1].AddGlow();
            if (items[items.Count - 1].GetItemId() == items[i].GetItemId())
            {
                items[i].AddGlow();
                temp++;
            }
            else
            {
                return;

            }


        }


    }

    public void GenerateLoadingSpots()
    {
        loadingSpots.Clear();

        for (int i = 0; i < containerData.totalItemsCanHold; i++)
        {
            loadingSpots.Add(new Vector3(sp.transform.position.x, sp.transform.position.y, sp.transform.position.z + ((i + 1) * gapBetweenItems)));
        }

        if (debugMode.isDebugMode & spawnDebugobj)
        {
            DebuggingTools.SpawnDebugObjs(debugMode.debugCube, loadingSpots, sp.transform.rotation, sp.transform);

        }

    }



}
