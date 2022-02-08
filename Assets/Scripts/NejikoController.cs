using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;

    public int Life()
    {
        return life;
    }
    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;//recoverTimeに値が入ってるかライフが0か
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();//this.gameObject.GetComponent<CharacterController>();の略
        animator = GetComponent<Animator>();
    }

    void Update()//フレーム数によって回数が違う
    {
        if (Input.GetKeyDown("left"))//GetKey ボタンを押してる間 GetKeyUp ボタンを離した時 GetKeyDown ボタンを押したときだけ
        {
            MoveToLeft();
        }
        if (Input.GetKeyDown("right"))
        {
            MoveToRight();
        }
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }

        if (IsStun())
        {
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;//1秒間をフレーム数で割った数がTime.deltaTime 厳密に言うと1フレームを描画する時間　実際の1秒間のフレーム数は均等ではない　フレーム数が違う機種によっての差をなくす 
        }
        else
        {
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);//Time.deltaTime 0.0168
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;//
            moveDirection.x = ratioX * speedX;//speedX 設定した3　ねじこが左右に移動する速さ
        }

        moveDirection.y -= gravity * Time.deltaTime;//moveDirectionがマイナスになったらねじ子が落ちてくる
        Vector3 globalDirection = transform.TransformDirection(moveDirection);//なくてもいい
        controller.Move(globalDirection * Time.deltaTime);

        if (controller.isGrounded)//地面に接触していたらTrue  isGroundedはCharacterControllerが持っているプロパティ
        {
            moveDirection.y = 0;
        }
        animator.SetBool("run", moveDirection.z > 0f);

    }
    public void MoveToLeft()
    {
        if (IsStun())
        {
            return;
        }
        if (controller.isGrounded && targetLane > MinLane)
        {
            targetLane--;
        }
    }
    public void MoveToRight()
    {
        if (IsStun())
        {
            return;
        }
        if (controller.isGrounded && targetLane < MaxLane)
        {
            targetLane++;
        }
    }
    public void Jump()
    {
        if (IsStun())
        {
            return;
        }
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("jump");
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)//characterController で動かすか rigitbodyで動かすか　 characterControllerがあるオブジェクトと衝突した時の判定 OnTrrigerEnter OnCollitionEnter
    {
        if (IsStun())
        {
            return;
        }
        if(hit.gameObject.tag == "Robo")
        {
            life--;
            recoverTime = StunDuration;

            animator.SetTrigger("damage");

            Destroy(hit.gameObject);
        }
    }
}



/*
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();//this.gameObject.GetComponent<CharacterController>();の略
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            MoveToLeft();
        }
        if (Input.GetKeyDown("right"))
        {
            MoveToRight();
        }
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }

        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

        float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
        moveDirection.z = ratioX * speedX;

        /*
        if (controller.isGrounded)//地面に接触していたらTrue  isGroundedはCharacterControllerが持っているプロパティ
        {
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;//Verticalは上下　上方向の座標を前方向に変換
            }
            else
            {
                moveDirection.z = 0;
            }

            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);//Horizontal　→を押すと1 ←を押すと-1

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;
                animator.SetTrigger("jump");
            }
        }
        /

        moveDirection.y -= gravity * Time.deltaTime;//Time.deltaTime1フレームを描画するのにかかる時間 どの端末でも時間差が出ないようにするために必要

        Vector3 globalDirection = transform.TransformDirection(moveDirection);//方向を考慮した方向に進む
        controller.Move(globalDirection * Time.deltaTime);//Move関数 globalDirection*0.168 1秒間あたりの進む距離

        if (controller.isGrounded)
        {
            moveDirection.y = 0;
        }

        animator.SetBool("run", moveDirection.z > 0.0f);//animator見た目のコンポーネント moveDirection.z > 0.0f 今正の方向にボタンが押されていればTrue
            }

                public void MoveToLeft()
        {
            if (controller.isGrounded && targetLane > MinLane)
            {
                targetLane--;
            }
        }
        public void MoveToRight()
        {
            if (controller.isGrounded && targetLane < MaxLane)
            {
                targetLane++;
            }
        }
        public void Jump()
        {
            if (controller.isGrounded)
            {
                moveDirection.y = speedJump;

                animator.SetTrigger("Jump");
            }
       }
}
 
 */