using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    private Vector3 movementDirection;
    private Rigidbody rigidbody;
    
    [Header("Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float speed;
    private void Awake()
    {
        instance = this;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        rigidbody.velocity = movementDirection * speed;
    }
    public void TeleportInstant(Vector3 worldSpacePosition)
    {
        transform.position = worldSpacePosition;
    }
    public void SetMovementDirection(Vector3 value)
    {
        movementDirection = value;
    }

    public System.Text.StringBuilder debugGetStats()
    {
        return new System.Text.StringBuilder(uiDebug.str_playerTitle)
            .Append(uiDebug.str_targetFPS).Append(Game.instance.targetFramerate)
            .Append("\nhealth = ").Append(currentHealth).Append(" / ").Append(maxHealth)
            .Append("\nspeed = ").Append(speed)
            .Append("\nmovementInput = ").Append(movementDirection.ToStringBuilder());
    }
}
