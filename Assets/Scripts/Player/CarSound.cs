using UnityEngine;

public class CarSound : MonoBehaviour
{
    [Header("References")]
    public Rigidbody car_rb;
    public AudioSource car_Audio;
    public AudioSource car_Siren;

    [Header("Settings")]
    public float min_Speed = 0f;
    public float max_Speed = 60f; // Adjust this to your car's top speed
    public float min_Pitch = 0.5f; // Deep idle sound
    public float max_Pitch = 2.5f; // High-speed revving sound

    private void Start()
    {
        //Police Siren audio playing
        car_Siren.Play();
    }
    void Update()
    {
        if (car_rb == null || car_Audio == null) return; // Prevents errors if components are missing

        float current_Speed = car_rb.linearVelocity.magnitude;

        // 1. Calculate how fast we are going as a percentage (0 to 1)
        float speedFactor = Mathf.InverseLerp(min_Speed, max_Speed, current_Speed);

        // 2. Smoothly set the pitch between min and max based on that percentage
        car_Audio.pitch = Mathf.Lerp(min_Pitch, max_Pitch, speedFactor);

        // 3. (Optional) Increase volume slightly as you go faster for "power"
        car_Audio.volume = Mathf.Lerp(0.5f, 1.0f, speedFactor);

        
    }
}