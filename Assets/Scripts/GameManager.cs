using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public RamFighter ramA;
    public RamFighter ramB;
    public GameObject damagePopupPrefab;

    private bool gameEnded = false;

    void Awake() => Instance = this;

    void Update()
    {
        if (gameEnded) return;

        if (ramA.stats.health <= 0)
            EndGame("Winner: Ram B");
        else if (ramB.stats.health <= 0)
            EndGame("Winner: Ram A");
    }

    public void ShowDamage(Vector3 position, int amount, Color color, bool isHealing)
    {
        var popup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        popup.GetComponent<DamagePopup>().Show(amount, color, isHealing);
    }


    void EndGame(string winner)
    {
        gameEnded = true;
        UIManager.Instance.ShowWinner(winner);
        ramA.StopFighting();
        ramB.StopFighting();
    }
}
