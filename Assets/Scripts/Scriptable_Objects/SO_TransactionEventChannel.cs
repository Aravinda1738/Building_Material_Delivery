using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TransactionEventChannel", menuName = "Scriptable Objects/SO_TransactionEventChannel")]
public class SO_TransactionEventChannel : ScriptableObject
{
    public event Action<Container> onMove;


    public void OnMoveAction(Container container)
    {
        onMove?.Invoke(container);
    }
}
