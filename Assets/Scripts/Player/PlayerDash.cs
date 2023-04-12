using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed;
    public float dashDuration;

    private float dashTime;
    private bool isDashing;
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isDashing)
        {
            if(dashTime < dashDuration)
            {
                dashTime += Time.deltaTime;
                myRigidbody.velocity = transform.right * dashSpeed;
            }
            else
            {
                isDashing = false;
                dashTime = 0f;
            }
        }
        
    }

    public void Dash(Vector2 direction)
    {
        if(!isDashing)
        {
            transform.right = direction;
            isDashing = true;
        }
    }

    
}
