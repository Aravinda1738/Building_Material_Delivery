using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;

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


    private int matchingCount = 0;
    private float moveSpeed = 0.005f;

    public bool isMovingOut = false;

    public bool spawnDebugobj = false;

    public string test = "tooo";

    private Vector3 moveToPos;
    // [Tooltip("you can add only 4 elements")]

   // private Stack<DeleveryItem> loadedItems = new Stack<DeleveryItem>();
    private List<LoadingSpots> loadingSpots = new List<LoadingSpots>();
    private List<GameObject> pickedGroup = new List<GameObject>();

    private int noOfOccupiedSpots =0;



    List<GameObject> recevedItems = new List<GameObject>();
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
            loadingSpots[i].occupent=temp;
            // GameObject temp = Instantiate(debugMode.debugCube, item, sp.transform.rotation);
            temp.name = $"Item_{i}";
            // temp.transform.localScale = new Vector3(.5f, .5f, .5f);
            temp.transform.SetParent(sp.transform);
            //loadedItems.Push(temp.GetComponent<DeleveryItem>());

            noOfOccupiedSpots++;
        }


    }



    public void PickAction()
    {

        if (TransactionManager.Instance.IsSenderAvailable())
        {
           
            //check if unload is possable


            bool loadSuccessful = Load();

            if (!loadSuccessful && this.containerId != TransactionManager.Instance.GetSender().containerId)
            {

                if(noOfOccupiedSpots != 0)
                {
                   

                    PrepairToSend();
                    Unload();
                }
            }

            //1Done //if empty -> DROP
            //2Done //if celected Container 1 and container 2 are same return the items , dose not cost a move
            //3Drop//if picked item & top item of the 2nd cotainer are same & there is  enough space-> DROP

            //4if 2nd cotainer is not empty and the top item not same -> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP
            //5if 2nd cotainer has same type of item on top but its full or dont have enough space-> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP

        }
        else
        {
            // pickup the top bunch
            if (noOfOccupiedSpots != 0)
            {
               

                PrepairToSend();
                Unload();

            }

        }




    }


    public bool Load()
    {
        this.recevedItems = TransactionManager.Instance.GetItemsToTransfer();
        int occupiedSpots = 0;
        bool result = true;
        Container recever = this;

        if (noOfOccupiedSpots != 0)
        {
            Debug.LogError("0000000000000000000" + this.recevedItems.Count);
            if (this.containerId == TransactionManager.Instance.GetSender().containerId ||
                !TopItemMatchCheck(this.recevedItems[0].GetComponent<DeleveryItem>(), loadingSpots[noOfOccupiedSpots-1].occupent.GetComponent<DeleveryItem>()))
            {
                result = false;
                Debug.LogError("111111111111111111111");
                recever = TransactionManager.Instance.GetSender();//to return to sender if not possable    4
            }

        }
        foreach (GameObject recevedItem in this.recevedItems)
        {

            // load in empty spots
            if (this.recevedItems.Count <= recever.loadingSpots.Count || recever.noOfOccupiedSpots == 0)
            {

                for (int i = 0; i < recever.loadingSpots.Count; i++) //find a spot to drop in recever
                {


                    Debug.LogError("22222222222222222222222-----" + recever.loadingSpots[i].isOccupied);
                    if (!recever.loadingSpots[i].isOccupied)
                    {
                        recevedItem.transform.parent = recever.sp.transform;
                        recevedItem.transform.position = recever.loadingSpots[i].spot;

                        recevedItem.name = $"Item_{i}";
                        recevedItem.GetComponent<DeleveryItem>().RemoveGlow();
                        Debug.LogError("22222222222222222222222");
                        recever.loadingSpots[i].occupent=recevedItem;
                        recever.loadingSpots[i].isOccupied = true;

                        occupiedSpots++;
                        break;
                    }
                    occupiedSpots++;
                }
            }




        }
        TransactionManager.Instance.SetSenderIsAvailable(false);
        this.recevedItems.Clear();
        //if (this.containerId == TransactionManager.Instance.GetSender().containerId)
        //{
        //    this.PickAction();
        //}
        DebuggingTools.PrintMessage($" END Load CALLED Receved Items -> {recevedItems.Count} Occupied Spots {occupiedSpots}/{recever.containerData.totalItemsCanHold}", this);
        return result;
    }
    public void PrepairToSend()
    {
        FindValidItemsAndPick();

        TransactionManager.Instance.SetSenderIsAvailable(true);
        TransactionManager.Instance.SetSender(this);
    }

    public void Unload()
    {
        TransactionManager.Instance.StoreSendingItems(pickedGroup);

            Debug.LogWarning(" - 2 ======" + pickedGroup.Count);

    }
    public void MoveOut()
    {
        isMovingOut = true;




    }

    public void FindValidItemsAndPick()
    {
        int temp = noOfOccupiedSpots - 1;

        loadingSpots[noOfOccupiedSpots - 1].occupent.GetComponent<DeleveryItem>().AddGlow();
        pickedGroup.Clear();
        pickedGroup.Add(loadingSpots[temp].occupent);
        loadingSpots[temp].isOccupied = false;
        loadingSpots[temp].occupent=null;
        noOfOccupiedSpots--;

        //loadedItems.Peek().AddGlow();
        //pickedGroup.Add(loadedItems.Peek().gameObject);
        //loadingSpots[loadingSpots.Count - 1].isOccupied = false;
        //loadedItems.Pop();


            Debug.LogWarning(loadingSpots.Count+"777777777777777777"+pickedGroup.Count);
       
        for (int i = noOfOccupiedSpots - 1; i >= 0; i--)
        {


            if (loadingSpots[i].occupent == null)
            {
                break; // Stop if we find an empty spot in the stack
            }
            if (TopItemMatchCheck(loadingSpots[i].occupent.GetComponent<DeleveryItem>(), pickedGroup[0].GetComponent<DeleveryItem>()))
            {
                loadingSpots[i].occupent.GetComponent<DeleveryItem>().AddGlow();
                loadingSpots[i].isOccupied = false;
                loadingSpots[i].occupent = null;
                pickedGroup.Add(loadingSpots[i].occupent);
                noOfOccupiedSpots--;
            }
            else
            {
                break;
            }
        }
       
        Debug.LogWarning("++++++++++++"+pickedGroup.Count);
        //for (int i = loadedItems.Count; i > 0; i--)
        //{

        //    if (TopItemMatchCheck(loadedItems.Peek(), pickedGroup[i].GetComponent<DeleveryItem>()))
        //    {

        //        loadedItems.Peek().AddGlow();
        //        loadingSpots[i].isOccupied = false;
        //        pickedGroup.Add(loadedItems.Peek().gameObject);
        //        loadedItems.Pop();
        //        temp++;

        //    }
        //    else
        //    {
        //        return temp;

        //    }


        //}



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

        Debug.LogError($"{A.GetItemId() == b.GetItemId()}");
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
}

public class LoadingSpots
{
    public Vector3 spot = new Vector3(0, 0, 0);
    public bool isOccupied = false;
    public GameObject occupent ;

    public LoadingSpots(Vector3 spot, bool isOccupied)
    {
        this.spot = spot;
        this.isOccupied = isOccupied;
        
    }
}
