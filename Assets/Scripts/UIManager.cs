using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider ramAHealth;
    [SerializeField] private Slider ramBHealth;
    [SerializeField] private TMP_Text winnerText;

    public void UpdateHealthBars(GoatStats ramAStats, GoatStats ramBStats)
    {
        ramAHealth.value = ramAStats.GetHealthPercent();
        ramBHealth.value = ramBStats.GetHealthPercent();
    }

    public void ShowWinner(string player)
    {
        winnerText.text = "Winner: " + player;
        winnerText.gameObject.SetActive(true);
    }
}
