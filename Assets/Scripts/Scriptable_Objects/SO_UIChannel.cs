using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_UIChannel", menuName = "Scriptable Objects/SO_UIChannel")]
public class SO_UIChannel : ScriptableObject
{
    public event Action onStartGame;
    public event Action<bool> onNextLevel;
    public event Action<int,int> onUpdateLevelText;
    public event Action onBackToHome;
    public event Action onUnDo;
    public event Action onAddExtraContainer;
    public event Action<int> onUpdateMovesLeft;



    public void OnStartGame()
    {
        onStartGame?.Invoke();
    }
    public void OnNextLevel(bool isLevelComplete)
    {
        onNextLevel?.Invoke(isLevelComplete);
    }



    public void OnUpdateLevelText(int level,int movesAvailable)
    {
        onUpdateLevelText?.Invoke(level,movesAvailable);
    }


    public void OnBackToHomeAction()
    {
        onBackToHome?.Invoke();
    }
    public void OnAddExtraContainer()
    {
        onAddExtraContainer?.Invoke();
    }
    public void OnUnDo()
    {
        onUnDo?.Invoke();
    }
    public void OnUpdateMovesLeft(int movesLeft)
    {
        onUpdateMovesLeft?.Invoke(movesLeft);
    }
}
