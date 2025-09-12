using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{
  

    public List<GameObject> itemTypes;

    public ItemType itemType;



}
public enum ItemType
{
    CEMENT,
    STEEL_ROD,
    BRICK,
    SAND
}