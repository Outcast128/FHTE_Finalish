using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle,
    dash,
    dead
}

public class Player : MonoBehaviour
{
    public static Player instance;

    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 movement;
    private Animator animator;

    public float maxHealth;
    public FloatValue currentHealth;
    public GameSignal playerHealthSignal;

    public Inventory playerInventory;
    public SpriteRenderer recievedItemSprite;

    public static Player GetInstance()
    {
        if (instance == null)
        {
            instance = new Player();
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(instance);
        }
        return instance;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentState = PlayerState.walk;
        currentHealth.initialValue = maxHealth;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        if (currentState == PlayerState.interact)
        {
            return;
        }
        movement = Vector3.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    public float dashTimer = 1f;

    private float dashTimerTracker = 0f;

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time > dashTimerTracker)
        {
            dashTimerTracker = Time.time + dashTimer;

            myRigidbody.AddForce(new Vector2(500f, 500f), ForceMode2D.Force);
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                animator.SetBool("recieve item", true);
                currentState = PlayerState.interact;
                recievedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("recieve item", false);
                currentState = PlayerState.idle;
                recievedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }

    void UpdateAnimationAndMove()
    {
        if (movement != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        movement.Normalize();
        myRigidbody.MovePosition(
            transform.position + movement * speed * Time.fixedDeltaTime
        );
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth.RuntimeValue -= damageAmount;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue <= 0)
        {
            Die();
        }
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }


    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void Die()
    {
        currentState = PlayerState.dead;
        animator.SetTrigger("death");
    }

    public void Heal(float healAmount)
    {
        currentHealth.RuntimeValue += healAmount;
        if (currentHealth.RuntimeValue > maxHealth)
        {
            currentHealth.RuntimeValue = maxHealth;
        }
        playerHealthSignal.Raise();
    }
}