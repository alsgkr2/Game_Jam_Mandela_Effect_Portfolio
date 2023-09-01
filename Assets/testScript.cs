using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public void Clear(int gameIdx)
    {
        GameManager.GetInstance().ReturntoStory(gameIdx);
    }
}
