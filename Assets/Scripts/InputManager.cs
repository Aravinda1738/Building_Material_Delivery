using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private SO_InputReader inputReader;
    [SerializeField]
    private SO_DebugMode debugMode;

    private bool canProcessTouch = true;

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

        if (canProcessTouch)
        {
           canProcessTouch = false;
            if (Physics.Raycast(ray, out hit))
            {
                Container container = hit.collider.GetComponent<Container>();
               
                if (container != null)
                {

                    
                    canProcessTouch = TransactionManager.Instance.PickAction(container);
                }
                else
                {
                    canProcessTouch = true;

                }


                if (debugMode.isDebugMode)
                {

                    DebuggingTools.PrintMessage($" Touched object: {hit.transform.name}", this);
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
                }
            }

        }
        else
        {
            if (debugMode.isDebugMode)
            {


                DebuggingTools.PrintMessage("red", " In Process ", this);
            }
        }


    }
}
