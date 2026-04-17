using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform cameraTransform;
    public float distance = 2f;
    public float followSpeed = 5f;
    public float verticalOffset = 0f;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        if (cameraTransform != null) SnapToCamera();
    }

    void LateUpdate()
    {
        if (cameraTransform == null || !gameObject.activeSelf) return;
        transform.position = Vector3.Lerp(transform.position, TargetPos(), followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot(), followSpeed * Time.deltaTime);
    }

    public void SnapToCamera()
    {
        if (cameraTransform == null) return;
        transform.position = TargetPos();
        transform.rotation = TargetRot();
    }

    Vector3 TargetPos()
    {
        Vector3 fwd = cameraTransform.forward;
        fwd.y = 0f;
        if (fwd.sqrMagnitude < 0.001f) fwd = Vector3.forward;
        fwd.Normalize();
        return cameraTransform.position + fwd * distance + Vector3.up * verticalOffset;
    }

    Quaternion TargetRot()
    {
        Vector3 dir = transform.position - cameraTransform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return Quaternion.identity;
        return Quaternion.LookRotation(dir);
    }
}
