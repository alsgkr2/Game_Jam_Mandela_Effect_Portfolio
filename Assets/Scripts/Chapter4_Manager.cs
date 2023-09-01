using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chapter4_Manager : MonoBehaviour
{
    public GameObject clearScreen, gameoOverScreen, startScreen;
    public AudioClip[] clipss;
    AudioSource audioS;
    GameObject canvas;
    public GameObject[] wards;
    GameObject[] gameWards;
    public Text text;
    //피버모드 관련
    public bool feverMode;
    float feverModeCool;
    float feverModeTime;
    float feverModeEnemeAttackCool;
    Image[] feverGauge;
    GameObject feverGameObjects;
    GameObject answerImage;
    //창문 관련
    float nextDayTime;
    Image windowImage;
    public Image[] windowImageArray;
    //만델라 이미지 관련
    Image mandelaImage;
    public Image[] mandelaImageArray;
    //대통령 이미지 관련
    Image presidentImage;
    public Image[] presidentImageArray;
    //적 이미지 관련
    Image enemyImage;
    public Image[] EnemyImageArray;
    //달력 관련
    Text calendarText;
    float calendarCool;
    int calendarMonth;
    int calendarDay;


    
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        canvas = GameObject.Find("Canvas");
        gameWards = new GameObject[5];
        //피버모드 관련
        feverModeCool = 5;
        feverModeTime = 5;
        feverGauge = new Image[2];
        feverGauge[0] = canvas.transform.Find("FeverImage1").GetComponent<Image>();
        feverGauge[1] = canvas.transform.Find("FeverImage2").GetComponent<Image>();
        feverMode = false;
        feverModeEnemeAttackCool = 0.5f;
        feverGameObjects = canvas.transform.Find("FeverObjects").gameObject;
        answerImage = GameObject.Find("AnswerImage");
        //창문 관련
        nextDayTime = 6f;
        windowImage = GameObject.Find("WindowImage").GetComponent<Image>();
        //만델라 이미지 관련
        mandelaImage = GameObject.Find("MandelaImage").GetComponent<Image>();
        //대통령 이미지 관련
        presidentImage = GameObject.Find("PresidentImage").GetComponent<Image>();
        //적 이미지 관련
        enemyImage = GameObject.Find("EnemyImage").GetComponent<Image>();
        //달력 관련
        calendarText = GameObject.Find("CalendarText").GetComponent<Text>();
        calendarMonth = 12;
        calendarDay = 1;
        calendarCool = 1;
        Time.timeScale = 0f;
        StartCoroutine(UntilPressSpace());
    }
    IEnumerator UntilPressSpace()
    {
        var delay = new WaitForSecondsRealtime(0.06f);
        while (Time.timeScale < 0.5f)
        {
            yield return delay;
            if (Input.GetKey(KeyCode.Space)) break;
        }
        startScreen.SetActive(false);
        delay = new WaitForSecondsRealtime(1f);
        for (int i = 0; i < 3; i++)
        {
            text.text = 3 - i + "" ;
            yield return delay;
        }
        text.text = "";
        StartGame();
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        
    }
        public void ReStart()
    {
        SceneManager.LoadScene("Game4_Liberation_day");
    }
    void Update()
    {
        //패배 조건 충족
        if (feverGauge[0].fillAmount <= 0)
        {

            gameoOverScreen.SetActive(true);
            return;
        }

        //게임 피버 모드 체크
        Chapter4Game(feverMode);

        //창문 실시간 이미지 변경
        nextDayTime -= Time.deltaTime;
        if (nextDayTime < 2) windowImage.sprite = windowImageArray[2].sprite;
        else if(nextDayTime < 4) windowImage.sprite = windowImageArray[1].sprite;
        else windowImage.sprite = windowImageArray[0].sprite;

        //창문 시간 초기화
        if (nextDayTime <= 0) nextDayTime = 6;

        //날짜 변경
        calendarCool -= Time.deltaTime;
        if (calendarCool <= 0)
        {
            if (calendarDay == 30)
            {
                calendarDay = 1;
                switch (calendarMonth)
                {
                    case 12:
                        calendarMonth = 1;
                        break;
                    case 1:
                        calendarMonth = 2;
                        break;
                    default:
                        break;
                }
            }
            else calendarDay++;
            calendarCool = 1;
            
        }

        //클리어 조건 충족
        if (calendarMonth == 2 && calendarDay == 11)
        {
            for (int i = 0; i < gameWards.Length; i++)
            {
                Destroy(gameWards[i]);
                gameWards[i] = null;
            }
            clearScreen.SetActive(true);
            return;
        }

        //만델라, 대통령, 적 실시간 이미지 변경
        if (feverGauge[0].fillAmount > 0.75f)
        {
            mandelaImage.sprite = mandelaImageArray[0].sprite; // 만델라 건강
            presidentImage.sprite = presidentImageArray[0].sprite; // 대통령 의기양양
            enemyImage.sprite = EnemyImageArray[0].sprite; // 적 위축
        }
        else if (feverGauge[0].fillAmount > 0.5f)
        {
            mandelaImage.sprite = mandelaImageArray[1].sprite; // 만델라 보통
            presidentImage.sprite = presidentImageArray[1].sprite; // 대통령 보통
            enemyImage.sprite = EnemyImageArray[1].sprite; // 적 보통
        }
        else if (feverGauge[0].fillAmount > 0.25f)
        {
            mandelaImage.sprite = mandelaImageArray[2].sprite; // 만델라 쇠약
            presidentImage.sprite = presidentImageArray[2].sprite; // 대통령 위축
            enemyImage.sprite = EnemyImageArray[2].sprite; // 적 의기양양
        }
        else if (feverGauge[0].fillAmount <= 0) mandelaImage.sprite = mandelaImageArray[3].sprite; // 만델라 사망

        calendarText.text = calendarMonth + "월\n" + calendarDay + "일";
    }

    public void Chapter4Game(bool feverMode)
    {
        if (!feverMode)
        {
            feverGauge[0].fillAmount -= 0.1f * Time.deltaTime;
            feverGauge[1].fillAmount += 0.1f * Time.deltaTime;

            for (int i = 0; i < gameWards.Length; i++)
            {
                if (gameWards[i] == null && i != 4)
                {
                    gameWards[i] = gameWards[i + 1];
                    gameWards[i + 1] = null;
                }
                else if (gameWards[i] == null && i == 4)
                {
                    gameWards[i] = Instantiate(wards[Random.Range(0, 4)], new Vector2(1928, 630), Quaternion.identity);
                    gameWards[i].transform.parent = canvas.transform;
                }

                if (i == 0 && gameWards[i] != null) gameWards[i].transform.position = new Vector2(1128, 630);
                else if (i == 1 && gameWards[i] != null) gameWards[i].transform.position = new Vector2(1328, 630);
                else if (i == 2 && gameWards[i] != null) gameWards[i].transform.position = new Vector2(1528, 630);
                else if (i == 3 && gameWards[i] != null) gameWards[i].transform.position = new Vector2(1728, 630);
            }

            string inputWard = null;

            if (Input.anyKeyDown)
            {
                audioS.clip = clipss[2];
                audioS.Play();
            }
            

            if (Input.GetKeyDown(KeyCode.UpArrow)) inputWard = "Up";
            else if (Input.GetKeyDown(KeyCode.DownArrow)) inputWard = "Down";
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputWard = "Left";
            else if (Input.GetKeyDown(KeyCode.RightArrow)) inputWard = "Right";
            //else if (Input.GetKeyDown(KeyCode.Space)) inputWard = "Space";

            if (gameWards[0] != null && inputWard == gameWards[0].tag && inputWard != null)
            {
                Destroy(gameWards[0]);
                gameWards[0] = null;
                feverGauge[0].fillAmount += 0.05f;
                feverGauge[1].fillAmount -= 0.05f;
            }
            else if (gameWards[0] != null && inputWard != gameWards[0].tag && inputWard != null)
            {
                Destroy(gameWards[0]);
                gameWards[0] = null;
                feverGauge[0].fillAmount -= 0.15f;
                feverGauge[1].fillAmount += 0.15f;
            }

            feverModeCool -= Time.deltaTime;
            if (feverModeCool <= 0)
            {
                for (int i = 0; i < gameWards.Length; i++)
                {
                    Destroy(gameWards[i]);
                    gameWards[i] = null;
                }
                answerImage.SetActive(false);
                feverGameObjects.SetActive(true);
                this.feverMode = true;
            }
        }
        else if (feverMode)
        {
            audioS.clip = clipss[0];
            audioS.Play();
            feverModeTime -= Time.deltaTime;

            //피버 모드 종료
            if (feverModeTime <= 0)
            {
                this.feverMode = false;
                feverModeCool = 5;
                feverModeTime = 5;
                feverModeEnemeAttackCool = 0.5f;
                feverGameObjects.SetActive(false);
                answerImage.SetActive(true);
            }

            //실시간 피버 게이지 증가 및 종료
            feverGauge[0].fillAmount -= 0.1f * Time.deltaTime;
            feverGauge[1].fillAmount += 0.1f * Time.deltaTime;

            //실시간 적 공격으로 게이지 증가 및 종료
            feverModeEnemeAttackCool -= Time.deltaTime;
            if (feverModeEnemeAttackCool <= 0) 
            {
                feverGauge[0].fillAmount -= 0.1f;
                feverGauge[1].fillAmount += 0.1f;
                feverModeEnemeAttackCool = 0.5f;
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                feverGauge[0].fillAmount += 0.05f;
                feverGauge[1].fillAmount -= 0.05f;
                audioS.clip = clipss[1];
                audioS.Play();
            }
        }
    }
}
