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

        // Ÿ��Ʋ ȭ�鿡�� ���� ������ ������ ���, �� ����� ������ �ΰ� Ŭ���� �� "���丮�� ���ư���" ��ư ����� �ʱ�
        GameManager.GetInstance().isDirectGame = true;
    }
}
