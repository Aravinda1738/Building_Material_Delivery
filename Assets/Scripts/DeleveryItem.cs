using System.Collections.Generic;
using UnityEngine;

public class DeleveryItem : MonoBehaviour
{

    [SerializeField]
    private SO_DebugMode debugMode;

    [SerializeField]
    private int id;
    [SerializeField]
    private SO_Container containerData;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    //public Stack<HistoryPoints> history = new Stack<HistoryPoints>();

    [SerializeField]
    private GameObject glow;




    public DeleveryItem(int type, GameObject owner)
    {
        this.id = type;
       // history.Push(new HistoryPoints(owner, transform.position));
    }

    private void Start()
    {


        if (debugMode.isDebugMode)
            DebuggingTools.PrintMessage($" Item Created with id:  {id}", this);
    }


    //public void GoBackToPreviousPoint()
    //{
    //    history.Pop();
    //    gameObject.transform.SetParent(history.Peek().preContainer.transform);
    //    // this.gameObject.transform.position=
    //}


    //public void RegisterMoveInHistory(GameObject owner, Vector3 currentLocationInContainer)// should be called after moving to new pos
    //{
    //    history.Push(new HistoryPoints(owner, currentLocationInContainer));
    //}


    private void OnValidate()
    {
        if (id > containerData.totalItemsCanHold)
        {
            id = -1;
            DebuggingTools.PrintMessage("ID must not be greater then containerData.totalItemsCanHold ", DebuggingTools.DebugMessageType.WARNING, this);
        }
    }



    public void AddGlow()
    {
        glow.SetActive(true);
    }

    public void RemoveGlow()
    {
        glow.SetActive(false);
    }
    public int GetItemId()
    { return id; }

    public void SetId(int newId)
    {

        id = newId;

    }
}
//public class HistoryPoints
//{
//    public GameObject preContainer;
//   // public Vector3 preLocationInContainer;
//    public Vector3 currentLocationInContainer;


//    public HistoryPoints(GameObject owner, Vector3 currentLocationInContainer)
//    {
//             preContainer = owner;
//        this.currentLocationInContainer = currentLocationInContainer;
//    }

//}
