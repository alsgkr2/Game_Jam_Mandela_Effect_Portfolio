using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class SaboTageUIManager : MonoBehaviour
{
    public Slider insertionTimer, timer;
    public TMP_Text startText;
    public Image timerFillImage, leftHumanImage,timerImage;
    public GameObject gameOverScreen, arrestedImage, spaceImage, startScreen, gameClearScreen;
    public static SaboTageUIManager instance;

    void Start()
    {
        instance = this;
    }
    public void StartTimer(int num)
    {
        if (num == -1) startText.text = "";
        else startText.text = num != 0 ? num.ToString() : "Start";
    }
    public void UpdateLife(int leftLife)
    { leftHumanImage.rectTransform.sizeDelta = new Vector2(86 * leftLife, 220); }
    public void UpdateInsertionTimer(float rate)
    {
        if (rate < 0.1f) spaceImage.SetActive(true);
        else
        {
            insertionTimer.gameObject.SetActive(true);
            spaceImage.SetActive(false);
            insertionTimer.value = rate;
            timerFillImage.color = Color.Lerp(Color.yellow, Color.green, rate);
        }
    }
    public void UpdateTimer(float rate)
    {
        timer.value = rate;
        timerImage.color = Color.Lerp(Color.yellow, Color.green, rate);
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
