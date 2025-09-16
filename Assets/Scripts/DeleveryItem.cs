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
        id = type;


      
    }

    private void Start()
    {
        if (debugMode.isDebugMode)
            Debug.Log("Item Created with id: " + id);
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
            Debug.LogWarning("ID must not be greater then containerData.totalItemsCanHold ");
        }
    }

}
