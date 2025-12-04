using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 2f; // Acceleration rate
    public float maxSpeed = 10f;    // Maximum speed
    public float deceleration = 5f; // Deceleration when no input

    private float speed = 0f;       // Current speed

    void Update()
    {
        // Check for input and handle acceleration
        if (Input.GetKey("w"))
        {
            // Accelerate
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, maxSpeed); // Limit speed to maxSpeed
        }
        else
        {
            // Decelerate when no input
            speed = Mathf.MoveTowards(speed, 0, deceleration * Time.deltaTime);
        }

        // Handle movement direction
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;

        // Move the cube with the current speed
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
