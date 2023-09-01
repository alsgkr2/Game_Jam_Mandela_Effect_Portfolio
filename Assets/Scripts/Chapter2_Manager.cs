using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter2_Manager : MonoBehaviour
{
    Camera camera;
    GameObject cameraAngleObject;

    GameObject prisonerMandela;
    GameObject prisoners;
    GameObject prisoners2;

    GameObject prisoners3;
    SpriteRenderer prisonerMandela_Close_eye;
    SpriteRenderer prisonerMandela_Open_eye;
    float close_Eye_Cool;
    float close_Eye_Time;
    bool close_Eye_Bool;

    Text captureNumText;
    int chaptureNum;
    int chaptureSuccess;

    int phaseNum;

    AudioSource cameraSound;

    GameObject resultImages;

    SpriteRenderer flash;
    float flashFloat;

    List<bool> resultBoolList;
    Image[] resultCheckImageArray;
    public Image[] checkIconArray;
    Image[] resultImageArray;
    public Image[] resultRandomFailImageArray;
    public Image[] resultSuccessImageArray;
    public GameObject startScreen, clearScreen, gameOverScreen;
    public Text text;
    bool canTake, allTrue;
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraAngleObject = GameObject.Find("CameraAngleObject");
        prisonerMandela = GameObject.Find("prisoner mandela1");
        prisoners = GameObject.Find("PrisonerObjects");
        prisoners2 = GameObject.Find("PrisonerObjects2"); prisoners2.SetActive(false);
        prisoners3 = GameObject.Find("PrisonerObjects3"); prisoners3.SetActive(false);
        prisonerMandela_Close_eye = GameObject.Find("mandela_close_eye").GetComponent<SpriteRenderer>();
        prisonerMandela_Open_eye = GameObject.Find("mandela_open_eye").GetComponent<SpriteRenderer>();
        captureNumText = GameObject.Find("CaptureNumText").GetComponent<Text>();
        resultImages = GameObject.Find("Canvas").transform.Find("ResultBackGround").gameObject;
        cameraSound = gameObject.GetComponent<AudioSource>();
        chaptureNum = 1;
        phaseNum = 1;
        close_Eye_Cool = 3;
        close_Eye_Time = 3;
        close_Eye_Bool = true;
        flash = GameObject.Find("Flash").GetComponent<SpriteRenderer>();
        resultBoolList = new List<bool>();
        resultCheckImageArray = new Image[3] { resultImages.transform.Find("ResultIcon1").GetComponent<Image>(),
            resultImages.transform.Find("ResultIcon2").GetComponent<Image>(), resultImages.transform.Find("ResultIcon3").GetComponent<Image>()};
        resultImageArray = new Image[3] { resultImages.transform.Find("ResultImage1").GetComponent<Image>(),
            resultImages.transform.Find("ResultImage2").GetComponent<Image>(), resultImages.transform.Find("ResultImage3").GetComponent<Image>()};
        Time.timeScale = 0f;
        StartCoroutine(UntilPressSpace());
    }
    IEnumerator UntilPressSpace()
    {
        canTake = false;
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
            text.text = 3 - i + "";
            yield return delay;
        }
        text.text = "";
        StartGame();
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        canTake = true;
    }
    void Update()
    {
        //현재 촬영 표시
        captureNumText.text = $"{chaptureNum}/3";

        if(flashFloat > 0) flashFloat -= 0.5f * Time.deltaTime;
        flash.color = new UnityEngine.Color(255, 255, 255, flashFloat);

        // 4번째 촬영 일시 결과창으로
        if (chaptureNum == 4)
        {
            allTrue = true;
            resultImages.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                 
                if (resultBoolList[i])
                {
                    
                    resultCheckImageArray[i].sprite = checkIconArray[0].sprite;
                    resultImageArray[i].sprite = resultSuccessImageArray[i].sprite;
                }
                else if (!resultBoolList[i])
                {
                    if (allTrue) allTrue = false;
                    resultCheckImageArray[i].sprite = checkIconArray[1].sprite;
                    if(i == 0) resultImageArray[i].sprite = resultRandomFailImageArray[Random.Range(0, 2)].sprite;
                    else if (i == 1) resultImageArray[i].sprite = resultRandomFailImageArray[Random.Range(2, 4)].sprite;
                    else if (i == 2) resultImageArray[i].sprite = resultRandomFailImageArray[4].sprite;
                }
            }
            chaptureNum++;
            
        }
        else
        {
            //페이즈에 따른 변화
            GamePhase(phaseNum);

            var mul = 7f;

            //카메라 앵글 키보드 이동
            if (Input.GetKey(KeyCode.UpArrow)) cameraAngleObject.transform.Translate(Vector2.up * Time.deltaTime * mul);
            else if (Input.GetKey(KeyCode.DownArrow)) cameraAngleObject.transform.Translate(Vector2.down * Time.deltaTime * mul);
            if (Input.GetKey(KeyCode.LeftArrow)) cameraAngleObject.transform.Translate(Vector2.left * Time.deltaTime * mul);
            else if (Input.GetKey(KeyCode.RightArrow)) cameraAngleObject.transform.Translate(Vector2.right * Time.deltaTime * mul);

            cameraAngleObject.transform.position = new Vector3(Mathf.Clamp(cameraAngleObject.transform.position.x, -15, 15),
                Mathf.Clamp(cameraAngleObject.transform.position.y, -1.5f, 1.5f), 0);

            //카메라 앵글 이동에 따른 카메라 이동(밖으로 못나가게)
            camera.transform.position = cameraAngleObject.transform.position;
            camera.transform.position = new Vector3(Mathf.Clamp(camera.transform.position.x, -15, 15), Mathf.Clamp(camera.transform.position.y, -1.5f, 1.5f), -10);

            //스페이스바 사진 촬영
            if (Input.GetKeyDown(KeyCode.Space) && canTake)
            {
                cameraSound.Play(); // 셔터음 재생
                flashFloat = 1;

                //사진 위치 계산
                var capturePoint = Vector2.Distance(prisonerMandela.transform.position, cameraAngleObject.transform.position);

                if (phaseNum == 3)
                {
                    if (capturePoint < 1)
                    {
                        Debug.Log(capturePoint + " " + close_Eye_Bool.ToString());
                        chaptureSuccess++;
                        Debug.Log("성공");
                        resultBoolList.Add(true);
                    }
                    else
                    {
                        Debug.Log(capturePoint + " " + close_Eye_Bool.ToString());
                        Debug.Log("실패");
                        resultBoolList.Add(false);
                    }
                }
                else
                {
                    if (capturePoint < 1)
                    {
                        Debug.Log(capturePoint);
                        chaptureSuccess++;
                        Debug.Log("성공");
                        resultBoolList.Add(true);
                    }
                    else
                    {
                        Debug.Log(capturePoint);
                        Debug.Log("실패");
                        resultBoolList.Add(false);
                    }
                }
                phaseNum++; // 페이즈 +1
                chaptureNum++; // 사진기회 +1
            }
        }
    }

    private void GamePhase(int phase)
    {
        switch(phase)
        {
            case 1:
                prisoners.transform.Translate(Vector2.left * 0.007f);
                break;
            case 2:
                prisoners.SetActive(false);
                prisoners2.SetActive(true);
                prisonerMandela = GameObject.Find("prisoner mandela2");
                break;
            case 3:
                prisoners2.SetActive(false);
                prisoners3.SetActive(true);
                close_Eye_Cool -= Time.deltaTime;
                if (close_Eye_Cool < 0)
                {
                    prisonerMandela.GetComponent<SpriteRenderer>().sprite = prisonerMandela_Close_eye.sprite;
                    close_Eye_Bool = false;
                    close_Eye_Time -= Time.deltaTime;
                }
                if (close_Eye_Time < 0)
                {
                    prisonerMandela.GetComponent<SpriteRenderer>().sprite = prisonerMandela_Open_eye.sprite;
                    close_Eye_Bool = true;
                    close_Eye_Cool = 3;
                    close_Eye_Time = 3;
                }
                prisonerMandela = GameObject.Find("prisoner mandela3");
                break;
            default:
                break;
        }
    }

    public void NextSceneButton()
    {
        if (allTrue) { clearScreen.SetActive(true); }
        else { gameOverScreen.SetActive(true); }
    }
    public void ReStart()
    {
        SceneManager.LoadScene("Game3_FindMandela");
    }
}
