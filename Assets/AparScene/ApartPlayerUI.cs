using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApartPlayerUI : MonoBehaviour
{
    public TMP_Text scoreText, startText;
    public Slider timer;
    public Image timerFillImage;
    public GameObject gameOverScreen, gameStartScreen, gameClearScreen;
    public static ApartPlayerUI instance;
    
    void Start()
    {
        instance = this;
    }
    public void UpdateTimer(float rate)
    {
        timer.value = rate;
        timerFillImage.color = Color.Lerp(Color.red, Color.green, rate);
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void StartTimer(int num)
    {
        if (num == -1) startText.text = "";
        else startText.text = num != 0 ? num.ToString() : "Start";
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
}