using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaboGameManager : MonoBehaviour
{
    public static SaboGameManager instance;
    public Transform playerRespawn;
    public List<BombInsertableObj> insertedObj = new List<BombInsertableObj>();
    public Sprite[] sprite;
    public GameObject arrowObj;
    AudioSource audio;
    int leftLife, leftCount;
    float timeLeft;
    void Start()
    {
        instance = this;
        leftLife = 2;
        leftCount = FindObjectsByType<BombInsertableObj>(FindObjectsSortMode.None).Length;
        Debug.Log(leftCount);
        Time.timeScale = 0f;
        timeLeft = 120f;
        StartCoroutine(UntilPressSpace());
        audio = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) GameOver();
        SaboTageUIManager.instance.UpdateTimer(timeLeft / 120f);
    }
    IEnumerator UntilPressSpace()
    {
        var delay = new WaitForSecondsRealtime(0.06f);
        while (Time.timeScale < 0.5f)
        {
            yield return delay;
            if (Input.GetKey(KeyCode.Space)) StartGame();
        }
    }
    public void StartGame() { SaboTageUIManager.instance.startScreen.SetActive(false); StartCoroutine(StartTimerActive(3)); }
    IEnumerator StartTimerActive(int leftTime)
    {
        WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(1f);
        while (leftTime-- >= 0)
        {
            yield return waitForSecondsRealtime;
            SaboTageUIManager.instance.StartTimer(leftTime);
        }
        Time.timeScale = 1f;
    }
    public void Arrest()
    {
        arrowObj.SetActive(false);
        StartCoroutine(MovePlayerRespawn());
        foreach (var item in insertedObj) item.BombRemoved();
        insertedObj.Clear();
        SaboTageUIManager.instance.arrestedImage.SetActive(true);
        SaboTageUIManager.instance.spaceImage.SetActive(false);
        SaboPlayerMove.instance.playerCollider.enabled = false;
        if (leftLife <= 0) { GameOver(); return; }
    }
    IEnumerator MovePlayerRespawn()
    {
        yield return new WaitForSeconds(5f);
        SaboPlayerMove.instance.playerCollider.enabled = true;
        SaboPlayerMove.instance.gameObject.transform.position = playerRespawn.position;
        SaboPlayerMove.instance.isArrest = false;
        SaboTageUIManager.instance.arrestedImage.SetActive(false);
        SaboTageUIManager.instance.UpdateLife(--leftLife);
    }
    public void FireAllBomb()
    {
        foreach (var item in insertedObj) item.BreakObj();
        audio.Play();
        StartCoroutine(CamShake.Shake());
        leftCount -= insertedObj.Count;
        if (leftCount == 0) GameClear();
        insertedObj.Clear();
        arrowObj.SetActive(false);
    }
    void GameClear()
    {
        SaboTageUIManager.instance.gameClearScreen.SetActive(true);
    }
    void GameOver()
    {
        SaboTageUIManager.instance.GameOver();
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game2_Sabotage");
    }
}
