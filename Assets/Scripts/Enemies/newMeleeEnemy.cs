using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public enum NewEnemyState
{
    idle,
    walk,
    attack,
    stagger,
    dead
}
public class newMeleeEnemy : MonoBehaviour
{
    public SkeletonAnimation anim;
    public NewEnemyState currentState;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public GameObject chest;

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = NewEnemyState.dead;
            anim.AnimationName = "Dead";
            this.gameObject.SetActive(false);
            chest.SetActive(true);
        }
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }


    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = NewEnemyState.idle;
            anim.AnimationName = "Idle";
            myRigidbody.velocity = Vector2.zero;
        }
    }

    public Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    // Start is called before the first frame update
    void Start()
    {
        currentState = NewEnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<SkeletonAnimation>();
        anim.AnimationName = "Idle";
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }
    public void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius
            && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == NewEnemyState.idle || currentState == NewEnemyState.walk
                && currentState != NewEnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(NewEnemyState.walk);
                anim.AnimationName = "Walk";
                //anim.SetBool("moving", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) <= chaseRadius
            && Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            if (currentState == NewEnemyState.idle || currentState == NewEnemyState.walk
                && currentState != NewEnemyState.stagger)
            {
                StartCoroutine(AttackCo());
            }
        }
        else
        {
            anim.AnimationName = "Idle";
            //anim.SetBool("moving", false);
        }
    }
    public IEnumerator AttackCo()
    {
        //anim.SetBool("moving", false);
        anim.AnimationName = "Idle";
        currentState = NewEnemyState.attack;
        //anim.SetBool("attacking", true);
        anim.AnimationName = "Attack";
        //anim.SetBool("attacking", false);
        yield return new WaitForSeconds(0.5f);
        currentState = NewEnemyState.walk;
        anim.AnimationName = "Walk";
    }

    public void SetAnimFloat(Vector2 setVector)
    {
        //anim.SetFloat("moveX", setVector.x);
        //anim.SetFloat("moveY", setVector.y);
    }


    public void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }


    public void ChangeState(NewEnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
