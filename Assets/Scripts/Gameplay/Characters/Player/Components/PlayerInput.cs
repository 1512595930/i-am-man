using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        JumpPressed = Input.GetButtonDown("Jump");
        JumpHeld = Input.GetButton("Jump");

        if (JumpPressed) Debug.Log("ÌøÔ¾¼ü°´ÏÂ");
    }
}