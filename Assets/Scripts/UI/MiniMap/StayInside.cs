using UnityEngine;

public class StayInside : MonoBehaviour
{
    public Transform MinimapCam;
    public float MinimapSize;

    private void Awake()
    {
        if(MinimapCam == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("MiniMapCamera");

            if(camObj != null )
            {
                MinimapCam = camObj.transform;
            }
        }
    }
    void LateUpdate()
    {
        // Center of minimap
        Vector3 centerPosition = MinimapCam.position;
        centerPosition.y -= 0.5f;

        // Real enemy position
        Vector3 realPos = transform.parent.position;
        realPos.y = centerPosition.y;

        float distance = Vector3.Distance(realPos, centerPosition);

        // Enemy outside minimap → clamp
        if (distance > MinimapSize)
        {
            Vector3 fromOriginToObject = realPos - centerPosition;
            fromOriginToObject *= MinimapSize / distance;
            transform.position = centerPosition + fromOriginToObject;
        }
        // Enemy inside minimap → true position
        else
        {
            transform.position = realPos;
        }
    }
}
