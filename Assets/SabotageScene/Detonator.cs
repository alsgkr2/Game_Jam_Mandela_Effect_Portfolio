using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision) { if (collision.gameObject.tag == "Player" && SaboPlayerMove.instance.isHoldSpace) { SaboTageUIManager.instance.spaceImage.gameObject.SetActive(false); if(SaboGameManager.instance.insertedObj.Count > 0) SaboGameManager.instance.FireAllBomb(); } }
    private void OnCollisionExit2D(Collision2D collision) { SaboTageUIManager.instance.spaceImage.gameObject.SetActive(false); }
    private void OnCollisionEnter2D(Collision2D collision) { if(SaboGameManager.instance.insertedObj.Count > 0) SaboTageUIManager.instance.spaceImage.gameObject.SetActive(true); }
}