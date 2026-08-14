
public class HealthSystem 
{
    int health_Max;
    int health;

    public HealthSystem(int health_Max)
    {
        this.health_Max = health_Max;
        health = health_Max;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GetHealthPercentage()
    {
        return (float) health/health_Max;
    }
    public void Damage(int damage)
    {
        health -= damage;
        if(health<=0)
            health = 0;
    }
}
