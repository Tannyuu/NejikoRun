using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    Vector3 diff;

    public GameObject target;
    public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        diff = target.transform.position - transform.position;//ベクトルの引き算
    }

    // Update is called once per frame
    void LateUpdate()//updateの直後に走る update→lateupdate→update... UodateもLateUpdateどちらも1秒間に60回
    {
        transform.position = Vector3.Lerp( //Lerp割合　A---B 0だとA 1だとB 0.5だと中間 2つの第三引数の割合を返す
            transform.position,
            target.transform.position - diff,//Nejikoのposition - Nejikoとカメラの距離
            Time.deltaTime * followSpeed//カメラが追いかけるスピード
            );
    }
}
