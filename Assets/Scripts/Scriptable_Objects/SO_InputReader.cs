using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SO_InputReader", menuName = "Scriptable Objects/SO_InputReader")]
public class SO_InputReader : ScriptableObject
{
    [SerializeField]
    private InputActionAsset inputAsset;


    private InputAction touchAction;
    private InputAction touchActionPos;

    public event UnityAction<Vector2> tapEvent;


    private void OnEnable()
    {

        touchAction = inputAsset.FindAction("Tap");
        touchActionPos = inputAsset.FindAction("TapPos");

        touchAction.performed += OnTouchPerformed;
        touchAction.canceled += OnResetTouch;

        touchActionPos.canceled += OnResetTouch;

        touchAction.Enable();
        touchActionPos.Enable();

    }

    private void OnDisable()
    {

        touchAction.performed -= OnTouchPerformed;
        touchAction.canceled -= OnResetTouch;
        touchActionPos.canceled -= OnResetTouch;

        touchAction.Disable();
        touchActionPos.Disable();

    }

   

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
       

        tapEvent?.Invoke(touchActionPos.ReadValue<Vector2>());
        touchAction.Reset();
        touchActionPos.Reset();
    }

    private void OnResetTouch(InputAction.CallbackContext context)
    {
        touchAction.Reset();
        touchActionPos.Reset();
    }
}
