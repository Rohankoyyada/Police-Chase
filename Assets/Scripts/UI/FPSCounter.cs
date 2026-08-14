using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float timer;
    private int frameCount;
    private float refreshRate = 0.5f; // update twice per second

    void Update()
    {
        frameCount++;
        timer += Time.unscaledDeltaTime;

        if (timer >= refreshRate)
        {
            float fps = frameCount / timer;
            fpsText.text =  Mathf.RoundToInt(fps).ToString();

            frameCount = 0;
            timer = 0f;
        }
    }
}
