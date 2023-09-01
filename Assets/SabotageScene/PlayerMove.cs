using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;
    public float speed = 3f;
    public bool isHoldSpace { get { return Input.GetKey(KeyCode.Space); } }
    public bool isArrest;
    public Sprite[] sprite;
    SpriteRenderer sr;
    Rigidbody2D rigid;
    Vector2 moveDir;
    void Start()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    
        isArrest = false;
    }
    void Update()
    {
        moveDir = isArrest ? Vector3.zero : (Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical")).normalized;
        rigid.velocity = moveDir * speed;
        sr.sprite = sprite[moveDir.y > 0.75f ? 1 : moveDir.y < -0.75f ? 2 : 0];
        sr.flipX = moveDir.x > 0;
    }
}
