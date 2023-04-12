using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : testEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius
            && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("moving", true);
            }
        }
        else if(Vector3.Distance(target.position, transform.position) <= chaseRadius
            && Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                StartCoroutine(AttackCo());
            }
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    public IEnumerator AttackCo()
    {
        anim.SetBool("moving", false);
        currentState = EnemyState.attack;
        anim.SetBool("attacking", true);
        yield return null;
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyState.walk;
    }
}
