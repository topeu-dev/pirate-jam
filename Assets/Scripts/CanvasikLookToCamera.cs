using UnityEngine;

public class CanvasikLookToCamera : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(-direction);
    }
}