using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    [Header("Camera Look")]
    public Transform PlayerCamera;
    public float LookSensitivity;
    public float MaxLookX;
    public float MinLookX;
    public InputAction LookAction;
    private float _camRotationX;

    public void HandleLook()
    {
        Vector2 lookInput = LookAction.ReadValue<Vector2>();
        Vector2 mouse = lookInput * LookSensitivity;
        transform.Rotate(0, mouse.x, 0);
        _camRotationX -= mouse.y;
        _camRotationX = Mathf.Clamp(_camRotationX, MinLookX, MaxLookX);
        PlayerCamera.localEulerAngles = new Vector3(_camRotationX, 0, 0);
    }
}
