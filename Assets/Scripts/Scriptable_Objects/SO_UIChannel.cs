using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_UIChannel", menuName = "Scriptable Objects/SO_UIChannel")]
public class SO_UIChannel : ScriptableObject
{
    public event Action onStartGame;
    public event Action onNextLevel;
    public event Action<int> onUpdateLevelText;
    public event Action onBackToHome;



    public void OnStartGame()
    {
        onStartGame?.Invoke();
    }
    public void OnNextLevel()
    {
        onNextLevel?.Invoke();
    }

    public void OnUpdateLevelText(int level)
    {
        onUpdateLevelText?.Invoke(level);
    }


    public void OnBackToHomeAction()
    {
        onBackToHome?.Invoke();
    }
}
