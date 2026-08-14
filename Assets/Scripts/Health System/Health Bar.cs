using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercentage(), 1);
    }
}
