using UnityEngine;

public class BrakeLightController : MonoBehaviour
{
    public Renderer brakeLightRenderer;
    public Color brakeColor = Color.red;
    public float brake_Light_Intensity = 2f;

    private Material brakeMat;

    void Start()
    {
        // Cache material once (important for performance)
        brakeMat = brakeLightRenderer.material;

        // Start with brake lights OFF
        brakeMat.DisableKeyword("_EMISSION");
    }

    public void SetBrakeLight(bool isBraking)
    {
        if (isBraking)
        {
            brakeMat.EnableKeyword("_EMISSION");
            brakeMat.SetColor("_EmissionColor", brakeColor * brake_Light_Intensity);
        }
        else
        {
            brakeMat.DisableKeyword("_EMISSION");
        }
    }
}
