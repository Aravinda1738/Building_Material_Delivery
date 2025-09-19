using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public  class TransactionManager : MonoBehaviour //Event invoker
{

    public static TransactionManager Instance {  get; private set; }

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
        isSenderAvailable=status;
    }    

    public void RegisterMove(GameObject movedItem)
    {
        //moves.Push(movedItem.GetComponent<DeleveryItem>());
    }


    public void StoreSendingItems(List<GameObject> sendingItems)
    {

        itemsToTransfer.Clear();
        itemsToTransfer = sendingItems;

        Debug.LogError("1-1-1-1-1-1-1-1-------"+ itemsToTransfer.Count);


    }



    public List<GameObject> GetItemsToTransfer() {

        DebuggingTools.PrintMessage($" 0 Load CALLED Receved Items -> {itemsToTransfer.Count}",this);
        return itemsToTransfer; 
    }
}


