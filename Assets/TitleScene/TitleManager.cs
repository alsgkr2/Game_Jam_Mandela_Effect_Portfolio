using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.LoadScene(GameScene.StoryScene);
    }

    public void StageStart(int idx)
    {
        GameManager.LoadGameScene(idx);

        // 타이틀 화면에서 직접 게임을 실행할 경우, 이 사실을 저장해 두고 클리어 시 "스토리로 돌아가기" 버튼 띄우지 않기
        GameManager.GetInstance().isDirectGame = true;
    }
}
