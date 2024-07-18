using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    InputActions input;

    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;

    private void Awake()
    {
        input = new InputActions();
    }

    // Update is called once per frame
    void Update()
    {
        input.Player.Horizontal.started += HorizontalInput;
        input.Player.Horizontal.performed += HorizontalInput;
        input.Player.Horizontal.canceled += HorizontalInput;

        input.Player.Vertical.started += VerticalInput;
        input.Player.Vertical.performed += VerticalInput;
        input.Player.Vertical.canceled += VerticalInput;
    }

    #region Get Functions
    public float GetHorizontalInput() {  return horizontalInput; }
    public float GetVerticalInput() {  return verticalInput; }
    #endregion

    #region Control Functions
    void HorizontalInput(InputAction.CallbackContext c)
    {
        horizontalInput = c.ReadValue<float>();
    }
    void VerticalInput(InputAction.CallbackContext c)
    {
        verticalInput = c.ReadValue<float>();
    }
    #endregion



    #region Enable/Disable
    private void OnEnable()
    {
        input.Player.Enable();
    }
    private void OnDisable()
    {
        input.Player.Disable();
    }
    #endregion
}
