using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaboPlayerMove : MonoBehaviour
{
    public static SaboPlayerMove instance;
    public float speed = 3f;
    public bool isHoldSpace { get { return Input.GetKey(KeyCode.Space); } }
    public bool isArrest, canMove;
    public Sprite[] sprite;
    public BoxCollider2D playerCollider;
    SpriteRenderer sr;
    Rigidbody2D rigid;

    Vector2 moveDir;
    void Start()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    
        isArrest = false;
        canMove = true;
    }
    void Update()
    {
        moveDir = isArrest || !canMove ? Vector3.zero : 
            (Vector3.right * (Input.GetKey(KeyCode.RightArrow) ? 1 : 0)
            + Vector3.left * (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0)
            + Vector3.up * (Input.GetKey(KeyCode.UpArrow) ? 1 : 0)
            + Vector3.down * (Input.GetKey(KeyCode.DownArrow) ? 1 : 0)).normalized;
        rigid.velocity = moveDir * speed;
        sr.sprite = sprite[moveDir.y > 0.75f ? 1 : moveDir.y < -0.75f ? 2 : 0];
        sr.flipX = moveDir.x > 0;
    }
}
