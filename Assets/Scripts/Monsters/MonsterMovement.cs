﻿using System.Collections;
using UnityEngine;

public abstract class MonsterMovement : MonoBehaviour
{
    public enum Status : int
    {
        Default = 0,
        Attack
    }

    public Rigidbody2D rigid = null;
    public Animator animator = null;
    public BoxCollider2D hitCollider = null;
    public float speed = 1f;

    private Status status = Status.Default;
    private float moveTime = 0f;
    private bool isMove = false;
    private float RangeX = 0f;
    private float waitTime = 0f;

    public void PlayMove()
    {
        animator.Play("Move", 0);
        isMove = true;
    }

    public virtual void Create(float rangeX)
    {
        RangeX = rangeX;
        moveTime = Random.Range(1.5f, 3f);
    }

    private void Update()
    {
        switch (status)
        {
            case Status.Default:
                CalculateMove();
                break;

            case Status.Attack:
                break;
        }
    }

    private void CalculateMove()
    {
        if (isMove) return;

        if (moveTime <= waitTime)
        {
            targetLocation = Random.Range(-RangeX, RangeX);

            if (targetLocation < 0) Rotate(true);
            else Rotate(false);

            waitTime = 0f;
            moveTime = Random.Range(1.5f, 3f);

            animator.Play("Move", 0);
            isMove = true;
            return;
        }

        waitTime += Time.deltaTime;
    }

    private void Rotate(bool isLeftRotate)
    {
        if (isLeftRotate) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.identity;

        isLeft = isLeftRotate;
    }

    private float targetLocation = 0f;
    private bool isLeft = false;
    private void FixedUpdate()
    {
        if (!isMove) return;

        switch (status)
        {
            case Status.Default:
                DefaultFixedMove();
                break;

            case Status.Attack:
                AttackFixedMove();
                break;
        }

        transform.Translate(speed * Time.fixedDeltaTime, 0, 0);
    }

    private void DefaultFixedMove()
    {
        if (isLeft)
        {
            if (targetLocation >= transform.localPosition.x)
                SetIdle();
        }
        else
        {
            if (targetLocation <= transform.localPosition.x)
                SetIdle();
        }

        void SetIdle()
        {
            animator.Play("Idle", 0);
            isMove = false;
        }
    }

    private void AttackFixedMove()
    {
        var t = CharacterModule.Get.transform;

        if (t.position.x > transform.position.x) Rotate(false);
        else Rotate(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.name.Equals("Effector"))
            {
                isMove = false;
                var vec = CharacterModule.Get.transform.position;


                if(OnHit(CharacterModule.Get.Damage))
                {
                    animator.Play("Death", 0);
                    rigid.simulated = false;

                    //var clips = animator.runtimeAnimatorController.animationClips;
                    //var clip = System.Array.Find(clips, x => x.name.Equals("Death"));
                    //Destroy(gameObject, clip.length);


                    Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
                }
                else
                {
                    animator.Play("Hit", 0);
                    status = Status.Attack;
                }

                if (vec.x > transform.position.x)
                    rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
                else
                    rigid.AddForce(Vector2.right, ForceMode2D.Impulse);

            }
        }
    }

    protected abstract bool OnHit(float damage);

}
