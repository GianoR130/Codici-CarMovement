using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    public float maxSpeedForward = 10f;
    public float maxSpeedReverse = 5f;
    public float acceleration = 8f;
    public float brakeForce = 20f;
    public float turnSpeed = 100f;

    private float currentSpeed = 0f;
    private float horizontalInput;
    private float verticalInput;

    private bool wallInFront = false;
    private bool wallBehind = false;

    private Rigidbody rb;

    public TMP_Text inputDisplay;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // --- LIMITA L’ACCELERAZIONE SE SEI CONTRO UN MURO ---
        if (wallInFront && verticalInput > 0)
            verticalInput = 0; // blocca accelerazione verso il muro

        if (wallBehind && verticalInput < 0)
            verticalInput = 0; // blocca retromarcia contro muro

        // --- ACCELERAZIONE / FRENATA ---
        if (verticalInput > 0) // avanti
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (verticalInput < 0) // indietro
        {
            if (currentSpeed > 0)
                currentSpeed -= brakeForce * Time.deltaTime; // frena
            else
                currentSpeed -= acceleration * Time.deltaTime; // retromarcia
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, acceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeedReverse, maxSpeedForward);

        // --- STERZO ---
        float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeedForward);
        float turn = horizontalInput * turnSpeed * speedFactor * Time.deltaTime;

        // correzione sterzo arcade: retro sterza nella stessa direzione
        if (currentSpeed < 0)
            turn = -turn;

        transform.Rotate(0, turn, 0);

        if (inputDisplay != null)
        {
            inputDisplay.text =
                $"Horizontal: {horizontalInput:F2}\nVertical: {verticalInput:F2}\nSpeed: {currentSpeed:F2}";
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckWallDirection(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        CheckWallDirection(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        wallInFront = false;
        wallBehind = false;
    }

    void CheckWallDirection(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;

        // muro davanti
        wallInFront = Vector3.Dot(normal, -transform.forward) > 0.5f;

        // muro dietro
        wallBehind = Vector3.Dot(normal, transform.forward) > 0.5f;

        // reset velocità solo verso il lato del muro
        if (wallInFront && currentSpeed > 0)
            currentSpeed = 0;

        if (wallBehind && currentSpeed < 0)
            currentSpeed = 0;
    }
}
