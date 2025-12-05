using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;            // Il cubo/macchina
    public Vector3 offset = new Vector3(0, 4, -6);

    public float smoothSpeed = 5f;

    // Look-ahead
    public float lookAheadAmount = 3f;
    public float lookAheadSpeed = 5f;

    // Tilt in curva
    public float tiltAmount = 8f;   // Gradi di inclinazione
    public float tiltSpeed = 5f;

    // Zoom dinamico
    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (!target) return;

        float speed = rb.linearVelocity.magnitude;

        // ==========================
        // 1) LOOK AHEAD (dove vai)
        // ==========================
        Vector3 lookAhead = target.forward * (speed / 10f) * lookAheadAmount;

        // ==========================
        // 2) ZOOM DINAMICO
        // ==========================
        float zoom = Mathf.Lerp(minZoom, maxZoom, speed / 20f);
        Vector3 zoomedOffset = offset;
        zoomedOffset.z = -zoom;

        // Posizione ideale
        Vector3 desiredPosition = target.position + target.rotation * zoomedOffset + lookAhead;

        // Movimento fluido
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;

        // ==========================
        // 3) TILT IN CURVA
        // ==========================
        float steeringTilt = Vector3.Dot(rb.linearVelocity, target.right);
        float tilt = steeringTilt * tiltAmount;

        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        desiredRot *= Quaternion.Euler(0, 0, -tilt);

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, tiltSpeed * Time.deltaTime);
    }
}
