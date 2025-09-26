using System.Collections.Generic;
using UnityEngine;

public class TransactionManager : MonoBehaviour //Event invoker
{

    public static TransactionManager Instance { get; private set; }

    [Header("Event Channel")]
    [SerializeField]
    private SO_TransactionEventChannel TransactionEventChannel;
    [SerializeField]
    private SO_UIChannel uIChannel;

    [SerializeField]
    private SO_Level_Manager levelData;

    [SerializeField]
    private SO_AudioChannel audioChannel;

    private bool isSenderAvailable = false;
    private bool isLevelEnd = false;
    private Container sender;
    private Stack<HistoryPoint> movesHistory = new Stack<HistoryPoint>(); //moves history for every bunch

    private List<GameObject> itemsToTransfer = new List<GameObject>();
    private List<int> completedContainerIds = new List<int>();

    private bool isTransferSuccess = false;
    private int totalMovesForThisLevel = 1;

    [SerializeField]
    private SO_DebugMode debugMode;
    private void OnEnable()
    {

        if (uIChannel != null)
        {
            uIChannel.onNextLevel += LevelRestart;
            uIChannel.onBackToHome += QuitLevel;
            uIChannel.onUnDo += UnDoMove;
            uIChannel.onUpdateLevelText += OnInitialUpdateLevelMoves;


        }
        else
        {
            debugMode.PrintMessage("UI Channel is empty", SO_DebugMode.DebugMessageType.ERROR, this);
        }


    }


    private void OnDisable()
    {

        if (uIChannel != null)
        {
            uIChannel.onNextLevel -= LevelRestart;
            uIChannel.onBackToHome -= QuitLevel;
            uIChannel.onUnDo -= UnDoMove;
            uIChannel.onUpdateLevelText -= OnInitialUpdateLevelMoves;


        }


    }







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


    public void AddCompletedContainerId(int id)
    {

        completedContainerIds.Add(id);


        if (completedContainerIds.Count == levelData.GetTotalTypesInGame())
        {

            isLevelEnd = true;
            QuitLevel();
            TransactionEventChannel.OnWinAction();

            debugMode.PrintMessage("green", "You Win!!! ", this);
        }

    }


    public void RemoveCompletedContainer(int id)
    {
        completedContainerIds.Remove(id);
    }

    public void OnInitialUpdateLevelMoves(int level, int movesAvailable)
    {
        totalMovesForThisLevel = movesAvailable;
    }



    private void LevelRestart(bool isLevelComplete)//bool not used here
    {
        isLevelEnd = false;

    }

    private void QuitLevel()
    {
        SetSender(null);
        GetItemsToTransfer().Clear();
        movesHistory.Clear();
        completedContainerIds.Clear();
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
        isTransferSuccess = false;
    }
    public void SendItemsToRecever(Container recever)
    {
        isTransferSuccess = true;
        recever.Load(GetItemsToTransfer());
        SetSenderIsAvailable(false);

        uIChannel.OnUpdateMovesLeft(totalMovesForThisLevel);

        //check if moveLeft is 0 GameOver
        if (totalMovesForThisLevel <= 0)
        {
            QuitLevel();
            TransactionEventChannel.OnGameOverAction();

        }
    }

    public void SetSenderAndReceveItems(Container sender, bool isSenderAvailable)
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

    public void ReleseItemsInHand()
    {
        if (!isTransferSuccess)
        {
            ReturnItemsToSender();


        }

    }


    /// <summary>
    /// While regestering "newOwner" is the is the "recever" and "oldOwner" is the "sender". 
    /// </summary>
    public void RegisterMove(List<GameObject> transferedItems, Container newOwner, Container oldOwner) //history
    {
        List<GameObject> temp = new List<GameObject>();
        transferedItems.ForEach(item => temp.Add(item));
        movesHistory.Push(new HistoryPoint(transferedItems, newOwner, oldOwner));
        totalMovesForThisLevel--;
        uIChannel.OnUpdateMovesLeft(totalMovesForThisLevel);
        debugMode.PrintMessage("blue", $" RegisterMove recever{movesHistory.Peek().GetRecever().name}  sender{movesHistory.Peek().GetSender().name} count {movesHistory.Peek().GetTransferedItems().Count}", this);

    }


    /// <summary>
    /// While UnDoMove()  the "recever" and the "sender" swap . 
    /// </summary>
    public void UnDoMove() //history
    {
        if (movesHistory.Count > 0)
        {

            debugMode.PrintMessage("blue", $"UNDO MOVE", this);
            HistoryPoint group = movesHistory.Peek();

            audioChannel.OnPickDrop();

            ReleseItemsInHand();

            SetSenderAndReceveItems(group.GetRecever(), group.GetTransferedItems(), false);//Sender

            SendItemsToRecever(group.GetSender());//Recever 
                                                  // GetItemsToTransfer().Clear();
            totalMovesForThisLevel++;
            uIChannel.OnUpdateMovesLeft(totalMovesForThisLevel);

            movesHistory.Pop();
        }
        else
        {
            debugMode.PrintMessage("blue", $"No more moves left", this);

        }

    }


    public bool PickAction(Container recever) //pick action is called in inputManager
    {
        bool result = false;




        //do nothing
        if (completedContainerIds.Contains(recever.GetContainerId()) || isLevelEnd)
        {
            return false;
        }

        if (IsSenderAvailable())
        {
            audioChannel.OnPickDrop();


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

                    RegisterMove(GetItemsToTransfer(), recever, GetSender());

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

                    SetSenderAndReceveItems(recever, true);
                    result = true;

                }
            }




        }
        else
        {


            SetSenderAndReceveItems(recever, true);
            isTransferSuccess = false;
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

