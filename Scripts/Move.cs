using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer  spriterenderer;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    { 
        rigid = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        anim =  GetComponent<Animator>();
    }
    void Update(){
        //점프
        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJumping")){
          
             rigid.AddForce(Vector2.up *jumpPower , ForceMode2D.Impulse);
             anim.SetBool("isJumping",true);
        }

        //가속력 멈춤
        if(Input.GetButtonUp("Horizontal")){
            
            rigid.velocity =  new Vector2(rigid.velocity.normalized.x * 0.5f,
                                                         rigid.velocity.y);
        }

        //플레이어 방향
        if(Input.GetButtonDown("Horizontal")){
            spriterenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        //애니메이샨(걷기)
        if(Mathf.Abs(rigid.velocity.x)<0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking",true);
    }

    // Update is called once per frame
    void FixedUpdate()// 1초에 50번 힘을 준다 (가속력이 붙는다. 상한가를 걸어야 한다.)
    {   
        //움직이는 속도
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right *h , ForceMode2D.Impulse);
        //최대가속력
        if (rigid.velocity.x >maxSpeed)// 오른쪽 최대 가속력
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1))// 왼쪽 최대 가속력
            rigid.velocity = new Vector2(maxSpeed*(-1),rigid.velocity.y);   
        //landing Platform 착지 플랫폼
        if(rigid.velocity.y < 0){
        //Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));//raycast값을 시각적으로 보여준 코드  
        RaycastHit2D rayHit =Physics2D.Raycast(rigid.position, Vector3.down,
                                                                1, LayerMask.GetMask("platform"));//레이캐스트
        //빔을 쏘고 맞으면 레이히트는 초기화
            if(rayHit.collider != null){
            if(rayHit.distance<0.5f)
            //Debug.Log(rayHit.collider.name); //레이 히트가 제대로 동작하고 있는 지 콘솔로 확인
                anim.SetBool("isJumping",false);
            }
        }
    }
}
