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
    private AudioClip audioStartEngine;
    [SerializeField]
    private AudioClip audioDrive;
    [SerializeField] 
    private AudioSource audioSource;

    [SerializeField]
    private float gapBetweenItems = 2;
    [SerializeField]
    private GameObject sp;

    [SerializeField]
    private GameObject turnOn;

    private int containerId;
    [SerializeField]
    private SO_TransactionEventChannel TransactionEventChannel;

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



    private void OnEnable()
    {
        if (TransactionEventChannel != null)
        {
            TransactionEventChannel.onWin += MoveOut;
        }
    }


    private void OnDisable()
    {
        if (TransactionEventChannel != null)
        {
            TransactionEventChannel.onWin -= MoveOut;
        }
    }

    //List<GameObject> recevedItems = new List<GameObject>();
    private void Start()
    {
        gameObject.name = $"Truck Number {containerId}";
        moveToPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 500);
        turnOn.SetActive(false);
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
            temp.name = $"Item_{i}-Of Type -> {temp.GetComponent<DeleveryItem>().GetItemTypeId()} in Con {containerId}";
            // temp.transform.localScale = new Vector3(.5f, .5f, .5f);
            temp.transform.SetParent(sp.transform);
            //loadedItems.Push(temp.GetComponent<DeleveryItem>());

            noOfOccupiedSpots++;
        }


    }




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

        }
        DebuggingTools.PrintMessage("yellow", $"After update Loaded items -> {noOfOccupiedSpots}", this);




        //winCheck


        if (noOfOccupiedSpots == loadingSpots.Count)
        {
            int complete = 0;
            for (int i = 1; i < loadingSpots.Count; i++)
            {

                if (loadingSpots[0].occupent.GetComponent<DeleveryItem>().GetItemTypeId() == loadingSpots[i].occupent.GetComponent<DeleveryItem>().GetItemTypeId())
                {
                    complete++;

                    if (complete == loadingSpots.Count - 1)
                    {
                        DebuggingTools.PrintMessage("green", "Complete", this);

                        TransactionManager.Instance.AddCompletedContainerId(containerId); // is end 
                        turnOn.SetActive(true);
                        audioSource.resource = audioStartEngine;
                        audioSource.volume = 0.9f;
                        audioSource.Play();

                    }
                }


            }
        }







    }


    public List<GameObject> Unload()
    {



        return FindValidItemsAndPick();

    }
    public List<GameObject> Unload(List<GameObject> unDoObjs)
    {
        List<GameObject> temp = new List<GameObject>();





        foreach (var obj in unDoObjs)
        {


            for (int i = loadingSpots.Count - 1; i >= 0; i--)
            {
                if (loadingSpots[i].isOccupied)
                {
                    if (loadingSpots[i].occupent.GetComponent<DeleveryItem>().GetItemTypeId() == obj.GetComponent<DeleveryItem>().GetItemTypeId())
                    {
                        temp.Add(loadingSpots[i].occupent);
                        loadingSpots[i].isOccupied = false;
                        loadingSpots[i].occupent = null;
                        noOfOccupiedSpots--;
                        break;
                    }
                }



            }
        }
        TransactionManager.Instance.RemoveCompletedContainer(containerId);



        turnOn.SetActive(false);



        return temp;

    }
    public void MoveOut()
    {
        audioSource.resource = audioDrive;
        audioSource.volume = 0.5f;
        audioSource.Play();
        turnOn.SetActive(true);
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


        if (A.GetItemTypeId() == b.GetItemTypeId())
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
        return loadingSpots.Count - noOfOccupiedSpots;
    }

    public int GetLoadedTopItemId()
    {
        for (int i = loadingSpots.Count - 1; noOfOccupiedSpots >= 0; i--)
        {
            if (loadingSpots[i].isOccupied)
            {
                return loadingSpots[noOfOccupiedSpots - 1].occupent.GetComponent<DeleveryItem>().GetItemTypeId();
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



        occupent = newOccupent;
        isOccupied = true;
        occupent.transform.parent = parent.transform;
        ResetToSpotPos();
        RemoveGlow();
    }


    public void RemoveOccupent()
    {
        occupent.GetComponent<DeleveryItem>().AddGlow();

        isOccupied = false;
        occupent = null;
    }

}
