using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Police : MonoBehaviour
{
    public float speed = 0.25f;
    float waitDelay;
    int nowPos = 0;
    bool isExcute;
    [SerializeField]
    Vector3[] movePoses;
    Vector3 moveDir;
    public Transform spriteTrans;
    public Sprite[] sprite;
    SpriteRenderer sr;
    WaitForSeconds delay;
    private void Start()
    {
        isExcute = false;
        delay = new WaitForSeconds(0.125f);
        movePoses = GetComponentsInChildren<Transform>()[2..].Select(x => x.position).ToArray();
        moveDir = (movePoses[nowPos] - transform.position);
        transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, moveDir) * (moveDir.y < 0 ? -1 : 1));
        spriteTrans.rotation = Quaternion.identity;
        waitDelay = 0;
        sr = spriteTrans.GetComponent<SpriteRenderer>();
        sr.sprite = sprite[moveDir.y > 0.75f ? 1 : moveDir.y < -0.75f ? 2 : 0];
        sr.flipX = moveDir.x > 0;
    }
    private void Update()
    {
        if (isExcute) return;
        if(waitDelay > 0) { waitDelay -= Time.deltaTime; return; }
        moveDir = (movePoses[nowPos] - transform.position);
        if (moveDir.magnitude < 0.5f)
        {
            waitDelay = 1f;
            nowPos = (nowPos + 1) % movePoses.Length;
            moveDir = (movePoses[nowPos] - transform.position).normalized;
            transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, moveDir) * (moveDir.y < 0 ? -1 : 1));
            spriteTrans.rotation = Quaternion.identity;
            sr.sprite = sprite[moveDir.y > 0.75f ? 1 : moveDir.y < -0.75f ? 2 : 0];
            sr.flipX = moveDir.x > 0;
        }
        transform.position += moveDir.normalized*Time.deltaTime*speed;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && SaboPlayerMove.instance.transform.position.y > -9f && !SaboPlayerMove.instance.isArrest) { 
            SaboPlayerMove.instance.isArrest = true;
            SaboGameManager.instance.Arrest();
            GetComponent<AudioSource>().Play();
            StartCoroutine(BringPlayer());
        }
    }
    IEnumerator BringPlayer()
    {
        isExcute = true;
        moveDir = ( SaboPlayerMove.instance.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, moveDir) * (moveDir.y < 0 ? -1 : 1));
        spriteTrans.rotation = Quaternion.identity;
        sr.sprite = sprite[moveDir.y > 0.75f ? 1 : moveDir.y < -0.75f ? 2 : 0];
        sr.flipX = moveDir.x > 0;
        SaboPlayerMove.instance.transform.parent = transform;
        for (int i = 0; i < 35; i++)
        {
            transform.position += moveDir.x < 0 ? Vector3.right : Vector3.left;
            SaboTageUIManager.instance.spaceImage.SetActive(false);
            yield return delay;
        }
        SaboPlayerMove.instance.transform.parent = null;
        Destroy(gameObject);
    }
}
    