using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RamFighter ramA, ramB;
    [SerializeField] private DamagePopup damagePopup;
    public UIManager ui;

    private bool gameEnded = false;

    void OnEnable()
    {
        ramA.ThrowResponse += DamageActivity;
        ramB.ThrowResponse += DamageActivity;
    }

    void OnDisable()
    {
        ramA.ThrowResponse -= DamageActivity;
        ramB.ThrowResponse -= DamageActivity;
    }

    void Update()
    {
        if (gameEnded) return;

        if (ramA.stats.health <= 0)
            EndGame(ramB.ramName);
        else if (ramB.stats.health <= 0)
            EndGame(ramA.ramName);
    }

    private void ShowDamage(Vector3 position, int amount, Color color, bool isHealing)
    {
        DamagePopup popup = Instantiate(damagePopup, position, Quaternion.identity);
        popup.Show(amount, color, isHealing);
    }


    void EndGame(string player)
    {
        gameEnded = true;
        ui.ShowWinner(player);
        ramA.StopFighting();
        ramB.StopFighting();
    }

    private void DamageActivity(int amount, RamFighter attacker)
    {
        RamFighter opponent = attacker.opponent;
        // Step 1: Opponent takes damage
        opponent.stats.TakeDamage(amount);

        // Step 2: Update health bars
        ui.UpdateHealthBars(attacker.stats, opponent.stats);

        // Step 3: Show red damage popup on opponent
        ShowDamage(opponent.damagePopupPoint.position, amount, Color.red, false);

        // Step 4: Heal attacker if opponent is still alive and not at max health
        if (opponent.stats.health > 0 && opponent.stats.health < opponent.stats.maxHealth)
        {
            attacker.stats.Heal(amount);
            ui.UpdateHealthBars(attacker.stats, opponent.stats);
            ShowDamage(attacker.damagePopupPoint.position, amount, Color.green, true);
        }
    }
}