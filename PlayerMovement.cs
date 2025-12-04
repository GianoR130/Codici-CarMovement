using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float maxSpeedForward = 10f;
    public float maxSpeedReverse = 5f;
    public float acceleration = 8f;
    public float deceleration = 12f;
    public float turnSpeed = 100f; // gradi al secondo
    public float brakeDeceleration = 20f;

    private float currentSpeed = 0f;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    void Update()
    {
        // Input
        horizontalInput = Input.GetAxis("Horizontal"); // A/D
        verticalInput = Input.GetAxis("Vertical");     // W/S

        // Accelerazione / decelerazione
        if (verticalInput > 0) // avanti
        {
            currentSpeed += acceleration * Time.deltaTime;
            if (currentSpeed > maxSpeedForward)
                currentSpeed = maxSpeedForward;
        }
        else if (verticalInput < 0) // retromarcia
        {
            currentSpeed -= acceleration * Time.deltaTime;
            if (currentSpeed < -maxSpeedReverse)
                currentSpeed = -maxSpeedReverse;
        }
        else // nessun input avanti/indietro
        {
            if (currentSpeed > 0)
                currentSpeed -= deceleration * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += deceleration * Time.deltaTime;

            // blocca la macchina se molto lenta
            if (Mathf.Abs(currentSpeed) < 0.05f)
                currentSpeed = 0f;
        }

        // Rotazione solo se la macchina si muove
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turn = horizontalInput * turnSpeed * Time.deltaTime * (currentSpeed / maxSpeedForward);
            transform.Rotate(0, turn, 0);
        }

        // Movimento in avanti nella direzione corrente
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
}
