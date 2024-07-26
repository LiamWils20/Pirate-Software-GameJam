using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance { get; private set; }
    public static PlayerInput input { get; private set; }
    private bool gameInputActive = true;
    private void Awake()
    {
        instance = this;
        input = new PlayerInput();
        //input.Player.Move.performed += ctx => Player.instance.SetMovementInput(ctx.ReadValue<Vector2>());
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Player.instance.SetMovementDirection(GetMovementDirection());
    }

    public void SetActive(bool value)
    {
        gameInputActive = value;
    }

    private Vector3 GetMovementDirection()
    {
        if (input.Player.Move.IsPressed())
        {
            Vector2 value = input.Player.Move.ReadValue<Vector2>();
            return new Vector3(value.x, 0.0f, value.y);
        }
        return default;
    }

    public bool GetInteract()
    {
        if (input.Player.Interact.IsPressed())
        {
            return true;
        }
        return default;
    }

    public bool GetDrink()
    {
        if (input.Player.Drink.IsPressed())
        {
            return true;
        }
        return default;
    }
    
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
