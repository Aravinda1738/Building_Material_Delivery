using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Level_Manager", menuName = "Scriptable Objects/SO_Level_Manager")]
public class SO_Level_Manager : ScriptableObject
{
    public int maxNumberOfParkingSpots;

    public float spaceBetween = 5f;

    public GameObject Vehical;

   

}
