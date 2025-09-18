using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private SO_InputReader inputReader;
    [SerializeField]
    private SO_DebugMode debugMode;

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.tapEvent += OnTap;
        }
    }
    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.tapEvent -= OnTap;
        }
    }



    private void OnTap(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (debugMode.isDebugMode)
            {

               DebuggingTools.PrintMessage($" Touched object: {hit.transform.name}", this);
 
            }
        }
    }
}
