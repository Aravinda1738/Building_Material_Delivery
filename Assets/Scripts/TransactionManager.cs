using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class TransactionManager : MonoBehaviour //Event invoker
{

    public static TransactionManager Instance { get; private set; }

    [Header("Event Channel")]
    [SerializeField]
    private SO_TransactionEventChannel TransactionEventChannel;
    [SerializeField]
    private SO_Level_Manager levelData;

    private bool isSenderAvailable = false;
    private Container sender;
    private Stack<HistoryPoint> movesHistory = new Stack<HistoryPoint>(); //moves history for every bunch

    private List<GameObject> itemsToTransfer = new List<GameObject>();
    private List<int> completedContainerIds = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       //Invoke("UnDoMove", 8f);
       //Invoke("UnDoMove", 12f);
       // Invoke("UnDoMove", 15f);
       // Invoke("UnDoMove", 20f);
    }


    public Container GetSender()
    {
        return this.sender;
    }

    public void SetSender(Container sender)
    {

        this.sender = sender;
    }

    public bool IsSenderAvailable()
    {
        return isSenderAvailable;
    }


    public void SetSenderIsAvailable(bool status)
    {
        isSenderAvailable = status;
    }


    public void AddCompletedContainerId(int id) { 
      
        completedContainerIds.Add(id);

        DebuggingTools.PrintMessage("red", $" completedContainerIds.Count{completedContainerIds.Count}  levelData.totalTypesInGame{levelData.totalTypesInGame} ", this);

        if (completedContainerIds.Count ==  levelData.totalTypesInGame)
        {
            TransactionEventChannel.OnWinAction();
        }
    }

    public void StoreSendingItems(List<GameObject> sendingItems)
    {

       // GetItemsToTransfer().Clear();
        SetItemsToTransfer(sendingItems);
    }

    public List<GameObject> GetItemsToTransfer() { return itemsToTransfer; }
    public void SetItemsToTransfer(List<GameObject> sendingItems) { itemsToTransfer = sendingItems; }

    public int GetItemsToTransferCount() { return itemsToTransfer.Count; }
    public int GetItemsToTransferItemId()
    {
        return GetItemsToTransfer()[0].GetComponent<DeleveryItem>().GetItemTypeId();
    }

    public void ReturnItemsToSender()
    {
        GetSender().Load(GetItemsToTransfer());
        SetSenderIsAvailable(false);
    }
    public void SendItemsToRecever(Container recever)
    {
        recever.Load(GetItemsToTransfer());
        SetSenderIsAvailable(false);
    }

    public void SetSenderAndReceveItems(Container sender,bool isSenderAvailable)
    {
        SetSender(sender);
        SetSenderIsAvailable(isSenderAvailable);
        StoreSendingItems(GetSender().Unload());
    }
    public void SetSenderAndReceveItems(Container sender, List<GameObject> undoObjs, bool isSenderAvailable)
    {
        SetSender(sender);
        SetSenderIsAvailable(isSenderAvailable);
        StoreSendingItems(GetSender().Unload(undoObjs));
    }

    /// <summary>
    /// While regestering "newOwner" is the is the "recever" and "oldOwner" is the "sender". 
    /// </summary>
    public void RegisterMove(List<GameObject> transferedItems, Container newOwner, Container oldOwner) //history
    {
        List<GameObject> temp = new List<GameObject>();
       transferedItems.ForEach(item => temp.Add(item));
        movesHistory.Push(new HistoryPoint(transferedItems, newOwner,oldOwner));
        DebuggingTools.PrintMessage("blue", $" RegisterMove recever{movesHistory.Peek().GetRecever().name}  sender{movesHistory.Peek().GetSender().name} count {movesHistory.Peek().GetTransferedItems().Count}" , this);
    }


    /// <summary>
    /// While UnDoMove()  the "recever" and the "sender" swap . 
    /// </summary>
    public void UnDoMove() //history
    {
        DebuggingTools.PrintMessage("blue", $"UNDO MOVE", this);
        HistoryPoint group = movesHistory.Peek();

        SetSenderAndReceveItems(group.GetRecever(),group.GetTransferedItems(),false);//Sender

        SendItemsToRecever(group.GetSender());//Recever 
       // GetItemsToTransfer().Clear();
        
        movesHistory.Pop();
    }


    public bool PickAction(Container recever) //pick action is called in inputManager
    {
        bool result = false;

        //do nothing
        if (completedContainerIds.Contains(recever.GetContainerId())) {
            return false;
        }

        if (IsSenderAvailable())
        {


            if (recever.GetContainerId() == GetSender().GetContainerId()) //clicked the same container  *
            {
                // return items to sender
                ReturnItemsToSender();
                //GetItemsToTransfer().Clear();
                result = true;

            }
            else //clicked a different container
            {


                if (recever.GetNoOfOccupiedSpots() == 0)   // recever is empty               *
                {
                    //  Just Drop

                    RegisterMove(GetItemsToTransfer(),recever, GetSender());

                    SendItemsToRecever(recever);
                   // GetItemsToTransfer().Clear();
                    result = true;

                }
                else if ((GetItemsToTransferItemId() == recever.GetLoadedTopItemId()) && (recever.GetNoOfFreeSpots() >= GetItemsToTransferCount()))
                {
                    //  Just Drop 

                    RegisterMove(GetItemsToTransfer(), recever, GetSender());

                    SendItemsToRecever(recever);
                   // GetItemsToTransfer().Clear();
                    result = true;
                }
                else if (recever.GetNoOfFreeSpots() < GetItemsToTransferCount() ||
                    GetItemsToTransferItemId() != recever.GetLoadedTopItemId())  // recever has not enough space or recever's top item and items to transfer are not same   *
                {
                    // Return picked objects to sender and pick the new top item set
                    ReturnItemsToSender();
                    //GetItemsToTransfer().Clear();

                    SetSenderAndReceveItems(recever,true);
                    result = true;

                }
            }




        }
        else
        {


            SetSenderAndReceveItems(recever,true);
            result = true;
        }


        return result;


    }

}
public class HistoryPoint
{
    private List<GameObject> transferedItems;
    private Container newOwner;
    private Container oldOwner;
    /// <summary>
    /// While regestering "newOwner" is the is the "recever" and "oldOwner" is the "sender". 
    /// </summary>
    public HistoryPoint(List<GameObject> transferedItems, Container newOwner, Container oldOwner)
    {
        this.transferedItems = transferedItems;
        this.newOwner = newOwner;
        this.oldOwner = oldOwner;
    }

    public List<GameObject> GetTransferedItems()
    {
        return transferedItems;
    }

    public Container GetRecever()
    {
        return newOwner;
    }
    public Container GetSender()
    {
        return oldOwner;
    }
}

