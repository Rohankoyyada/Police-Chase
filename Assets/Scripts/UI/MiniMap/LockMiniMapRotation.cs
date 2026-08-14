using UnityEngine;

public class LockMiniMapRotation : MonoBehaviour
{
    public Transform target;   // Police car
    public float fixedHeight = 40f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.z = target.position.z;
        pos.y = fixedHeight;   // 🔒 FIXED Y (no physics influence)
        transform.position = pos;
    }
}
