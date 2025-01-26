using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PDUIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject losePanel;

    private float timer = 60f; // 60 segundos para completar el nivel
    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        timerText.text = $"Tiempo: {Mathf.Ceil(timer)}s";

        if (timer <= 0)
        {
            EndGame(false);
        }
    }

    public void EndGame(bool won)
    {
        gameEnded = true;
        if (won)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }
}
