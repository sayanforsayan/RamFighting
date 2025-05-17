using UnityEngine;

public class GoatStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;

    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public float GetHealthPercent() => health / maxHealth;

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

}
