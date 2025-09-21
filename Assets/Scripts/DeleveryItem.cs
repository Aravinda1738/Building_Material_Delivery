using UnityEngine;

public class DeleveryItem : MonoBehaviour
{

    [SerializeField]
    private SO_DebugMode debugMode;

    [SerializeField]
    private int Id;
    [SerializeField]
    private SO_Container containerData;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    

    [SerializeField]
    private GameObject glow;




    public DeleveryItem(int typeId,  GameObject owner)
    {
        Id = typeId;
     
    }

    private void Start()
    {


        if (debugMode.isDebugMode)
            DebuggingTools.PrintMessage($" Item Created with id:  {Id}", this);
    }




    private void OnValidate()
    {
        if (Id > containerData.totalItemsCanHold)
        {
            Id = -1;
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
    public int GetItemTypeId()
    { return Id; }

    public void SetId(int newId)
    {

        Id = newId;

    }
}

