using System.Collections.Generic;
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

    private List<DeleveryItem> loadedItems = new List<DeleveryItem>();
    private List<LoadingSpots> loadingSpots = new List<LoadingSpots>();
    private List<GameObject> pickedGroup = new List<GameObject>();






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
            // GameObject temp = Instantiate(debugMode.debugCube, item, sp.transform.rotation);
            temp.name = $"Item_{i}";
            // temp.transform.localScale = new Vector3(.5f, .5f, .5f);
            temp.transform.SetParent(sp.transform);
            loadedItems.Add(temp.GetComponent<DeleveryItem>());
        }


    }



    public void PickAction()
    {


        if (TransactionManager.Instance.IsSenderAvailable())
        {
            //check if unload is possable

            Load();



            //1Done //if empty -> DROP
            //2Done //if celected Container 1 and container 2 are same return the items , dose not cost a move
            //3Drop//if picked item & top item of the 2nd cotainer are same & there is  enough space-> DROP

            //4if 2nd cotainer is not empty and the top item not same -> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP
            //5if 2nd cotainer has same type of item on top but its full or dont have enough space-> DROP THE PICKED ITEM FROM WHERE IT WAS PICKED AND PICK THE NEW TOP GROUP

        }
        else
        {
            // pickup the top bunch
            if (loadedItems.Count != 0)
            {

                PrepairToSend();
                Unload();

            }

        }




    }


    public void Load()
    {
        List<GameObject> recevedItems = TransactionManager.Instance.GetItemsToTransfer();
        int occupiedSpots = 0;

        Container recever = this;

        if (this.loadedItems.Count != 0)
        {
            if (this.containerId == TransactionManager.Instance.GetSender().containerId ||
                TopItemMatchCheck(recevedItems[0].GetComponent<DeleveryItem>(), this.loadedItems[this.loadedItems.Count-1]))
            {
                Debug.LogError("111111111111111111111");  
                recever = TransactionManager.Instance.GetSender();//to return to sender if not possable    4
            }

        }
        foreach (GameObject recevedItem in recevedItems)
        {

            // load in empty spots
            if (recevedItems.Count <= recever.loadingSpots.Count || recever.loadedItems.Count == 0)
            {

                for (int i = 0; i < recever.loadingSpots.Count; i++) //find a spot to drop in recever
                {


                    if (!recever.loadingSpots[i].isOccupied)
                    {
                        recevedItem.transform.parent = recever.sp.transform;
                        recevedItem.transform.position = recever.loadingSpots[i].spot;

                        recevedItem.name = $"Item_{i}";
                        recevedItem.GetComponent<DeleveryItem>().RemoveGlow();
                        Debug.LogError("22222222222222222222222");
                        recever.loadedItems.Add(recevedItem.GetComponent<DeleveryItem>());
                        recever.loadingSpots[i].isOccupied = true;

                        occupiedSpots++;
                        break;
                    }
                    occupiedSpots++;
                }
            }




        }
        TransactionManager.Instance.SetSenderIsAvailable(false);
        recevedItems.Clear();
        if (this.containerId == TransactionManager.Instance.GetSender().containerId)
        {
            this.PickAction();
        }

        DebuggingTools.PrintMessage($" END Load CALLED Receved Items -> {recevedItems.Count} Occupied Spots {occupiedSpots}/{recever.containerData.totalItemsCanHold}", this);
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
        for (int i = 0; i < pickedGroup.Count; i++)
        {
            for (int j = 0; j < loadedItems.Count; j++)
            {
                if (pickedGroup[i].GetComponent<DeleveryItem>().GetItemId() == loadedItems[j].GetItemId())
                {
                    loadedItems.Remove(loadedItems[j]);
                    loadingSpots[j].isOccupied = false;
                }

            }




        }


    }
    public void MoveOut()
    {
        isMovingOut = true;




    }

    public int FindValidItemsAndPick()
    {
        int temp = 0;
        pickedGroup.Clear();
        loadedItems[loadedItems.Count - 1].AddGlow();
        pickedGroup.Add(loadedItems[loadedItems.Count - 1].gameObject);

        for (int i = loadedItems.Count - 2; i > 0; i--)
        {

            temp++;
            if (TopItemMatchCheck(loadedItems[loadedItems.Count - 1], loadedItems[i]))
            {

                loadedItems[i].AddGlow();

                pickedGroup.Add(loadedItems[i].gameObject);

                temp++;

            }
            else
            {
                return temp;

            }


        }
        return temp;


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


    public LoadingSpots(Vector3 spot, bool isOccupied)
    {
        this.spot = spot;
        this.isOccupied = isOccupied;
    }
}
