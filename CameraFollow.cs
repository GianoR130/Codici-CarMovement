using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // il cubo/macchina da seguire
    public Vector3 offset = new Vector3(0, 5, -10); // posizione rispetto al target
    public float smoothSpeed = 0.1f; // velocità interpolazione

    void LateUpdate()
    {
        if (target == null) return;

        // posizione desiderata
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        // interpolazione per movimento fluido
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // guarda sempre il target
        transform.LookAt(target);
    }
}
