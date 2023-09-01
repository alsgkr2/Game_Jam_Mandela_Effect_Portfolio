using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInsertableObj : MonoBehaviour
{
    
    SpriteRenderer sr;
    AudioSource audio;
    bool isInserted;
    float leftTime;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();    
        isInserted = false;
        leftTime = 3f;
    }
    void BombInserted()
    {
        if (isInserted) return;
        isInserted = true;
        sr.sprite = SaboGameManager.instance.sprite[1];
        SaboGameManager.instance.insertedObj.Add(this);
        SaboGameManager.instance.arrowObj.SetActive(true);
        audio.Play();
    }
    public void BombRemoved()
    {
        isInserted = false;
        sr.sprite = SaboGameManager.instance.sprite[0];
        leftTime = 3f;
    }
    public void BreakObj() { GetComponent<Animator>().enabled = true; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (leftTime < 0) return;
        if (collision.gameObject.tag == "Player") { SaboTageUIManager.instance.spaceImage.gameObject.SetActive(true); leftTime = 3f; }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (leftTime < 0) return;
        if (collision.gameObject.tag == "Player") { SaboTageUIManager.instance.insertionTimer.gameObject.SetActive(false); SaboTageUIManager.instance.spaceImage.gameObject.SetActive(false); leftTime = 3f; }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isInserted) return;
        if (collision.gameObject.tag == "Player") {
            leftTime = SaboPlayerMove.instance.isHoldSpace && !SaboPlayerMove.instance.isArrest ? leftTime -= Time.deltaTime : 3f;
            SaboPlayerMove.instance.canMove = leftTime == 3f;
            SaboTageUIManager.instance.UpdateInsertionTimer((3f - leftTime) / 3f);
            if (leftTime < 0) { BombInserted(); SaboTageUIManager.instance.insertionTimer.gameObject.SetActive(false); SaboPlayerMove.instance.canMove = true; }
        }
    }
}
