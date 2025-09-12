using UnityEngine;

public class Container : MonoBehaviour
{
    public SO_Container containerData;

    private int containerLimit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        containerLimit = containerData.totalItemsCanHold;  
    }

    public void Load()
    {

    }

    public void Unload()
    {

    }
}
