using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlnoBehaviour : MonoBehaviour
{
    int nowDir;
    void Update()
    {
        nowDir = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow)) { ApartGameManager.instance.MoveHuman(1); nowDir++; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { ApartGameManager.instance.MoveHuman(-1); nowDir--; }
        Camera.main.transform.position += nowDir * Vector3.right * 0.5f;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x * 0.95f, 0, -10 );
    }
}
