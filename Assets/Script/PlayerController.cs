/*
Click_v1.06
    Version Target:
        1.新增飞行怪物
        2.用aseprite画一个简单的enemy
        
            

PS:素材是用aseprite画的


Time:


*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;


    Vector2 movementInput;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            //if movment input is not 0,try to move
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success && movementInput.x > 0)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success && movementInput.y > 0)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            }

            else
            {
                animator.SetBool("isMoving", false);
            }

            //Set direction of sprite to movement direction
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                
            }

            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                
            }


        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {

            //check for potential collisions
            int count = rb.Cast(
                    movementInput,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            //can't move if there's no direction to move in
            return false;
        }
    }



    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else  
        {
            swordAttack.AttackRight();
        }
        
        
    } 

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }
    public void LockMovement()
    {
        canMove = false;
    }
    public void UnlockMovement()
    {
        canMove = true;
    }
}
