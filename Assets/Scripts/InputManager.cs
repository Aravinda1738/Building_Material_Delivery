using UnityEngine;
using UnityEngine.EventSystems;

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



    private void Update()
    {
        if (Input.touchCount > 0)
        {
            
            Touch touch = Input.GetTouch(0);

            if (touch.phase== TouchPhase.Ended)
            {
              OnTap(touch.position);

            }


        }
    }


    private void OnTap(Vector2 touchPosition)
    {

        //if (EventSystem.current.IsPointerOverGameObject(UnityEngine.InputSystem.Touchscreen.current.primaryTouch.touchId.ReadValue()))
        //{
        //    return;
        //}


        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                Container container = hit.collider.GetComponent<Container>();
               
                if (container != null)
                {

                    
                     TransactionManager.Instance.PickAction(container);
                }
                


                if (debugMode.isDebugMode)
                {

                    DebuggingTools.PrintMessage($" Touched object: {hit.transform.name}", this);
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
                }
            }

        

    }
}
