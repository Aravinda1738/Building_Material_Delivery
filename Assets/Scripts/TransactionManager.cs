using System.Collections.Generic;
using UnityEngine;

public class TransactionManager : MonoBehaviour //Event invoker
{

    public static TransactionManager Instance { get; private set; }

    [Header("Event Channel")]
    [SerializeField]
    private SO_TransactionEventChannel TransactionEventChannel;


    private bool isSenderAvailable = false;
    private Container sender;
    private Stack<List<GameObject>> movesHistory = new Stack<List<GameObject>>(); //moves history

    private List<GameObject> itemsToTransfer = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
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

    public void RegisterMove(GameObject movedItem)
    {
        //moves.Push(movedItem.GetComponent<DeleveryItem>());
    }


    public void StoreSendingItems(List<GameObject> sendingItems)
    {

        GetItemsToTransfer().Clear();
        SetItemsToTransfer(sendingItems);
    }

    public List<GameObject> GetItemsToTransfer() { return itemsToTransfer; }
    public void SetItemsToTransfer(List<GameObject> sendingItems) { itemsToTransfer = sendingItems; }

    public int GetItemsToTransferCount() { return itemsToTransfer.Count; }
    public int GetItemsToTransferItemId()
    {
        return GetItemsToTransfer()[0].GetComponent<DeleveryItem>().GetItemId();
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

    public void SetSenderAndReceveItems(Container recever)
    {
        SetSender(recever);
        SetSenderIsAvailable(true);
        StoreSendingItems(GetSender().Unload());
    }

    public bool PickAction(Container recever) //pick action is called in inputManager
    {
        bool result = false;

        if (IsSenderAvailable())
        {


            if (recever.GetContainerId() == GetSender().GetContainerId()) //clicked the same container  *
            {
                // return items to sender
                ReturnItemsToSender();
                itemsToTransfer.Clear();
                result = true;

            }
            else //clicked a different container
            {


                if (recever.GetNoOfOccupiedSpots() == 0)   // recever is empty               *
                {
                    //  Just Drop 
                    SendItemsToRecever(recever);
                    itemsToTransfer.Clear();
                    result = true;

                }
                else if ((GetItemsToTransferItemId() == recever.GetLoadedTopItemId()) && (recever.GetNoOfFreeSpots() >= GetItemsToTransferCount()))
                {

                    //  Just Drop 
                    SendItemsToRecever(recever);
                    itemsToTransfer.Clear();
                    result = true;
                }
                else if (recever.GetNoOfFreeSpots() < GetItemsToTransferCount() ||
                    GetItemsToTransferItemId() != recever.GetLoadedTopItemId())  // recever has not enough space or recever's top item and items to transfer are not same   *
                {
                    // Return picked objects to sender and pick the new top item set
                    ReturnItemsToSender();
                    itemsToTransfer.Clear();

                    SetSenderAndReceveItems(recever);
                    result = true;

                }
            }




        }
        else
        {


            SetSenderAndReceveItems(recever);
            result = true;
        }


        return result;


    }

}


