using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    float dir;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            
            dir = Input.GetAxis("Horizontal");
            if (Input.GetAxis("Horizontal") > 0) ApartGameManager.instance.MoveHuman(1);
            if (Input.GetAxis("Horizontal") < 0) ApartGameManager.instance.MoveHuman(-1);

        }
    }
}
