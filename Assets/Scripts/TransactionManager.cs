using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    private GameObject sender;

    public GameObject getSender()
    {
        return sender;
    }

    public void setSender(GameObject sender)
    {

        this.sender = sender;
    }



}
