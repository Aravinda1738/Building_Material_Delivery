using UnityEngine;

public class DeleveryItem : MonoBehaviour
{

    [SerializeField]
    private SO_DebugMode debugMode;
    private int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetId()
    { return id; }

    public void SetId(int newId)
    { id = newId; }
}
