using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{
  

    public Mesh itemMesh;

    public ItemType itemType;



}
public enum ItemType
{
    CEMENT,
    STEEL_ROD,
    BRICK,
    SAND
}