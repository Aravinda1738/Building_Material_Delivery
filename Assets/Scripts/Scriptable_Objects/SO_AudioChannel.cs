using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_AudioChannel", menuName = "Scriptable Objects/SO_AudioChannel")]
public class SO_AudioChannel : ScriptableObject
{
    public event Action onPickDrop;
    public event Action onUiClick;



    public void OnPickDrop()
    {
        onPickDrop?.Invoke();
    }
    public void OnUiClick()
    {
        onUiClick?.Invoke();
    }
}
