using Unity.VisualScripting;
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
    public DeleveryItem(int type)
    {
       this.id = type;
    }

    private void Start()
    {
        if (debugMode.isDebugMode)

            DebuggingTools.PrintMessage($" Item Created with id:  {id}", this);
    }

    public int GetId()
    { return id; }

    public void SetId(int newId)
    {
        
        id = newId;
       
    }


    private void OnValidate()
    {
        if (id > containerData.totalItemsCanHold)
        {
            id = -1;
            DebuggingTools.PrintMessage("ID must not be greater then containerData.totalItemsCanHold ",DebuggingTools.DebugMessageType.WARNING ,this);
        }
    }

}
