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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTap(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {




            if (debugMode.isDebugMode)
            {

                Debug.Log(touchPosition + "Touched object: " + hit.transform.name);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
            }
        }
    }
}
