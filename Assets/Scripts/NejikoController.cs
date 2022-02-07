using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();//this.gameObject.GetComponent<CharacterController>();の略
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        moveDirection.y -= gravity * Time.deltaTime;//Time.deltaTime1フレームを描画するのにかかる時間 どの端末でも時間差が出ないようにするために必要

        Vector3 globalDirection = transform.TransformDirection(moveDirection);//方向を考慮した方向に進む
        controller.Move(globalDirection * Time.deltaTime);//Move関数 globalDirection*0.168 1秒間あたりの進む距離

        if (controller.isGrounded)
        {
            moveDirection.y = 0;
        }

        animator.SetBool("run", moveDirection.z > 0.0f);//animator見た目のコンポーネント moveDirection.z > 0.0f 今正の方向にボタンが押されていればTrue
    }
}
