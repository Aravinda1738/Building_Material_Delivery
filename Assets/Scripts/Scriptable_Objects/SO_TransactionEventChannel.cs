using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TransactionEventChannel", menuName = "Scriptable Objects/SO_TransactionEventChannel")]
public class SO_TransactionEventChannel : ScriptableObject
{
    public event Action<Container> onMove;
    public event Action onWin;
   
    public event Action onGameOver;


    public void OnMoveAction(Container container)
    {
        onMove?.Invoke(container);
    }
    public void OnWinAction()
    {
        onWin?.Invoke();
    }
    public void OnGameOverAction()
    {
        onGameOver?.Invoke();
    }


}
