using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScreenScript : MonoBehaviour
{
    public GameObject button;
    public int index;
    private void Start()
    {
        StartCoroutine(ButtonAppear());
        Time.timeScale = 0f;
    }
    IEnumerator ButtonAppear()
    {
        yield return new WaitForSecondsRealtime(2f);
        button.SetActive(true);
        StartCoroutine(UntilPressSpace());
    }
    IEnumerator UntilPressSpace()
    {
        var delay = new WaitForSecondsRealtime(0.125f);
        while (Time.timeScale < 0.5f)
        {
            yield return delay;
            if (Input.GetKey(KeyCode.Space)) ReturnToStory();
        }
    }
    public void ReturnToStory()
    {
        Time.timeScale = 1f;
        GameManager.GetInstance().ReturntoStory(index);
    }
}
