using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{

    [SerializeField]
    private List<GameObject> itemTypes;

   

    public GameObject GetItemType(int id)
    {
        return itemTypes[id];

    }
}
