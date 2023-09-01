using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    static public IEnumerator Shake()
    {
        WaitForSeconds delay = new WaitForSeconds(0.06f);
        for (int i = 0; i < 5; i++)
        {
            Camera.main.transform.position += (Vector3)Random.insideUnitCircle;
            yield return delay;
        }
        Camera.main.transform.position = Vector3.back * 10;
    }
}
