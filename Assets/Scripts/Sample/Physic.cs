using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physic : MonoBehaviour
{
    public Rigidbody2D rigid = null;
    public float force = 10f;
    public float maxCameraPosX;

    public void OnHit()
    {
        //벽에서 조금 튕겨나오게 조정 
        transform.position = new Vector3(
            maxCameraPosX - 0.01f, transform.position.y, 0);

        //튕길때의 힘 설정
        Vector2 velocity = new Vector2(-rigid.velocity.x, 100f * Time.fixedDeltaTime);

        // 힘 초기화 
        rigid.velocity = Vector3.zero;
        // 힘 적용
        rigid.velocity = velocity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            rigid.AddForce(new Vector2(force * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
    }

    private bool isHit = false;
    private void FixedUpdate()
    {
        if (isHit) return;
        if (maxCameraPosX < transform.position.x)
        {
            isHit = true;
            OnHit();
        }
    }
}
