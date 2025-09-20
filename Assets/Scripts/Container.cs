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


    private int containerId;


    // private int matchingCount = 0;
    private float moveSpeed = 0.005f;

    public bool isMovingOut = false;

    public bool spawnDebugobj = false;

    public string test = "tooo";

    private Vector3 moveToPos;
    // [Tooltip("you can add only 4 elements")]

    // private Stack<DeleveryItem> loadedItems = new Stack<DeleveryItem>();
    private List<LoadingSpots> loadingSpots = new List<LoadingSpots>();


    private int noOfOccupiedSpots = 0;



    //List<GameObject> recevedItems = new List<GameObject>();
    private void Start()
    {
        gameObject.name = $"Truck Number {containerId}";
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

        for (int i = 0; i < ids.Count; i++)
        {

            if (i > itemData.GetitemTypesCount())
            {
                DebuggingTools.PrintMessage(" id > item type ", DebuggingTools.DebugMessageType.ERROR, this);
            }

            GameObject temp = Instantiate(itemData.GetItemType(ids[i]), loadingSpots[i].spot, itemData.GetItemType(ids[i]).transform.rotation);
            loadingSpots[i].isOccupied = true;
            loadingSpots[i].occupent = temp;
            // GameObject temp = Instantiate(debugMode.debugCube, item, sp.transform.rotation);
            temp.name = $"Item_{i}-Of Type -> {temp.GetComponent<DeleveryItem>().GetItemId()}";
            // temp.transform.localScale = new Vector3(.5f, .5f, .5f);
            temp.transform.SetParent(sp.transform);
            //loadedItems.Push(temp.GetComponent<DeleveryItem>());

            noOfOccupiedSpots++;
        }


    }



    //public void PickAction()
    //{

    //    if (TransactionManager.Instance.IsSenderAvailable())
    //    {

    //        //check if unload is possable


    //        bool loadSuccessful = Load();

    //        if (!loadSuccessful && this.containerId != TransactionManager.Instance.GetSender().containerId)
    //        {

    //            if (noOfOccupiedSpots != 0)
    //            {



    //                Unload();
    //            }
    //        }

    //        //1Done //if empty -> DROP
    //        //2Done //if celected Container 1 and container 2 are same return the items , dose not cost a move
    //        //3Drop//if picked item & top item of the 2nd cotainer are same & there is  enough space-> DROP

    //        //4if 2nd cotainer is not empty and the top item not same -> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP
    //        //5if 2nd cotainer has same type of item on top but its full or dont have enough space-> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP

    //    }
    //    else
    //    {
    //        // pickup the top bunch
    //        if (noOfOccupiedSpots != 0)
    //        {



    //            Unload();

    //        }

    //    }




    //}


    public void Load(List<GameObject> itemsToLoad)
    {
        List<int> emptySpotIndexes = new List<int>(); //find empty spots

        for (int i = 0; i < loadingSpots.Count; i++)
        {

            if (!loadingSpots[i].isOccupied)
            {
                emptySpotIndexes.Add(i);
            }
        }
        DebuggingTools.PrintMessage($"Number of empty spots -> {emptySpotIndexes.Count} Loaded items -> {noOfOccupiedSpots}", this);


        for (int i = 0; i < itemsToLoad.Count; i++) //add to empty spots
        {

            loadingSpots[emptySpotIndexes[i]].AddNewOccupent(itemsToLoad[i], sp);

            noOfOccupiedSpots++;
            /*loadingSpots[emptySpotIndexes[i]].occupent = itemsToLoad[i];
           loadingSpots[emptySpotIndexes[i]].isOccupied = true;
            itemsToLoad[i].transform.parent = sp.transform;
            loadingSpots[emptySpotIndexes[i]].ResetToSpotPos();
            loadingSpots[emptySpotIndexes[i]].RemoveGlow();*/
        }
        DebuggingTools.PrintMessage("yellow", $"After update Loaded items -> {noOfOccupiedSpots}", this);

        //foreach (GameObject item in itemsToLoad)
        //{


        //    for (int i = loadingSpots.Count - 1; i >= 0; i--)
        //    {
        //        if (!loadingSpots[i].isOccupied)
        //        {
        //            item.transform.parent = sp.transform;
        //            item.transform.position = loadingSpots[i].spot;

        //            item.name = $"Item_{i}";
        //            item.

        //            loadingSpots[i].occupent = item;
        //            loadingSpots[i].isOccupied = true;

        //            noOfOccupiedSpots++;

        //        }


        //    }

        //}


        //recevedItems = TransactionManager.Instance.GetItemsToTransfer();
        //int occupiedSpots = 0;
        //bool result = true;
        //Container recever = this;

        //if (noOfOccupiedSpots != 0)
        //{

        //    if (this.containerId == TransactionManager.Instance.GetSender().containerId ||
        //        !TopItemMatchCheck(this.recevedItems[0].GetComponent<DeleveryItem>(), loadingSpots[noOfOccupiedSpots - 1].occupent.GetComponent<DeleveryItem>()))
        //    {
        //        result = false;

        //        recever = TransactionManager.Instance.GetSender();//to return to sender if not possable    4
        //    }

        //}
        //foreach (GameObject recevedItem in this.recevedItems)
        //{

        //    // load in empty spots
        //    if (this.recevedItems.Count <= recever.loadingSpots.Count || recever.noOfOccupiedSpots == 0)
        //    {

        //        for (int i = 0; i < recever.loadingSpots.Count; i++) //find a spot to drop in recever
        //        {



        //            if (!recever.loadingSpots[i].isOccupied)
        //            {
        //                recevedItem.transform.parent = recever.sp.transform;
        //                recevedItem.transform.position = recever.loadingSpots[i].spot;

        //                recevedItem.name = $"Item_{i}";
        //                recevedItem.GetComponent<DeleveryItem>().RemoveGlow();

        //                recever.loadingSpots[i].occupent = recevedItem;
        //                recever.loadingSpots[i].isOccupied = true;

        //                occupiedSpots++;
        //                break;
        //            }
        //            occupiedSpots++;
        //        }
        //    }




        //}

        //  TransactionManager.Instance.SetSenderIsAvailable(false);
        //  this.recevedItems.Clear();
        //if (this.containerId == TransactionManager.Instance.GetSender().containerId)
        //{
        //    this.PickAction();
        //}
        // DebuggingTools.PrintMessage($" END Load CALLED Receved Items -> {recevedItems.Count} Occupied Spots {occupiedSpots}/{recever.containerData.totalItemsCanHold}", this);
        //return result;
    }


    public List<GameObject> Unload()
    {

        return FindValidItemsAndPick();

    }
    public void MoveOut()
    {
        isMovingOut = true;

    }

    public List<GameObject> FindValidItemsAndPick()
    {
        int temp = noOfOccupiedSpots - 1;
        List<GameObject> pickedGroup = new List<GameObject>();


        loadingSpots[noOfOccupiedSpots - 1].occupent.GetComponent<DeleveryItem>().AddGlow();
        pickedGroup.Clear();
        pickedGroup.Add(loadingSpots[temp].occupent);
        loadingSpots[temp].isOccupied = false;
        loadingSpots[temp].occupent = null;
        noOfOccupiedSpots--;


        for (int i = noOfOccupiedSpots - 1; i >= 0; i--)
        {


            if (loadingSpots[i].occupent == null)
            {
                break; // Stop if we find an empty spot in the stack
            }
            if (TopItemMatchCheck(loadingSpots[i].occupent.GetComponent<DeleveryItem>(), pickedGroup[0].GetComponent<DeleveryItem>()))
            {
                loadingSpots[i].occupent.GetComponent<DeleveryItem>().AddGlow();
                pickedGroup.Add(loadingSpots[i].occupent);
                loadingSpots[i].isOccupied = false;
                loadingSpots[i].occupent = null;
                noOfOccupiedSpots--;
            }
            else
            {
                break;
            }
        }
        DebuggingTools.PrintMessage("cyan", $"Picked Group -> {pickedGroup}", this);
        return pickedGroup;
    }

    public void GenerateLoadingSpots()
    {
        loadingSpots.Clear();

        for (int i = 0; i < containerData.totalItemsCanHold; i++)
        {
            loadingSpots.Add(new LoadingSpots(new Vector3(sp.transform.position.x, sp.transform.position.y, sp.transform.position.z + ((i + 1) * gapBetweenItems)), false));

        }

        if (debugMode.isDebugMode & spawnDebugobj)
        {
            DebuggingTools.SpawnDebugObjs(debugMode.debugCube, loadingSpots, sp.transform.rotation, sp.transform);

        }

    }

    public bool TopItemMatchCheck(DeleveryItem A, DeleveryItem b)
    {

        if (A == null || b == null)
        {
            return false;
        }


        if (A.GetItemId() == b.GetItemId())
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public int GetContainerId()
    {
        return containerId;
    }
    public void SetId(int id)
    {
        containerId = id;
    }

    public int GetNoOfOccupiedSpots()
    {
        return noOfOccupiedSpots;
    }
    public int GetNoOfFreeSpots()
    {
        return   loadingSpots.Count- noOfOccupiedSpots;
    }

    public int GetLoadedTopItemId()
    {
        for (int i = loadingSpots.Count-1; noOfOccupiedSpots >=0; i--)
        {
            if (loadingSpots[i].isOccupied)
            {
                return loadingSpots[noOfOccupiedSpots - 1].occupent.GetComponent<DeleveryItem>().GetItemId();
            }
            
        }

        return -1;
       
    }

}

public class LoadingSpots
{
    public Vector3 spot = new Vector3(0, 0, 0);
    public bool isOccupied = false;
    public GameObject occupent;

    public LoadingSpots(Vector3 spot, bool isOccupied)
    {
        this.spot = spot;
        this.isOccupied = isOccupied;

    }

    public void ResetToSpotPos()
    {

        occupent.transform.position = spot;

    }


    public void RemoveGlow()
    {
        occupent.GetComponent<DeleveryItem>().RemoveGlow();
    }


    public void AddNewOccupent(GameObject newOccupent, GameObject parent)
    {
        if (newOccupent == null)
        {
            DebuggingTools.PrintMessage("newOccupent is NULL in AddNewOccupent", DebuggingTools.DebugMessageType.ERROR, this);
            return;
        }


        if (newOccupent == null)
        {

            DebuggingTools.PrintMessage("newOccupent is NULL in AddNewOccupent", DebuggingTools.DebugMessageType.ERROR, this);
            return;
        }

       // occupent.GetComponent<DeleveryItem>().RegisterMoveInHistory(parent,spot);

        occupent = newOccupent;
        isOccupied = true;
        occupent.transform.parent = parent.transform;
        ResetToSpotPos();
        RemoveGlow();
    }


}
