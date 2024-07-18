using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] PlayerStats stats;
    [SerializeField] float speed;
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        input = gameObject.GetComponent<PlayerInput>();
        stats = gameObject.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = stats.GetSpeed();
        horizontalInput = input.GetHorizontalInput();
        verticalInput = input.GetVerticalInput();

        Vector3 inputFromPlayer = new Vector3 (verticalInput, 0, horizontalInput);
        inputFromPlayer.Normalize();
        
        transform.Translate (inputFromPlayer * speed * Time.deltaTime);
    }
}
