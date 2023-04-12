using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public int maxHealth;
    public int currentHealth;
    public int maxInventorySlots = 6;

    public struct InventoryItem
    {
        public string itemName;
        public int itemAmount;
    }

    public InventoryItem[] inventory;

    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownTimeLeft;
    private Animator anim;
    private Vector2 lastMovement = Vector2.zero;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        inventory = new InventoryItem[maxInventorySlots];

        for (int i = 0; i < maxInventorySlots; i++)
        {
            inventory[i].itemName = "";
            inventory[i].itemAmount = 0;
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(moveX, moveY);
            if (movement != Vector2.zero)
            {
                lastMovement = movement;
                anim.SetFloat("moveX", movement.x);
                anim.SetFloat("moveY", movement.y);
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && dashCooldownTimeLeft <= 0f)
            {
                StartCoroutine(Dash());
            }

            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
                dashCooldownTimeLeft = dashCooldown;
            }
            else
            {
                dashTimeLeft -= Time.deltaTime;
                transform.Translate(transform.position * dashSpeed * Time.deltaTime);
            }
        }

        if (dashCooldownTimeLeft > 0f)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("attacking", true);
        }
        else
        {
            anim.SetBool("attacking", false);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        yield return null;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }

    public void AddToInventory(string itemName, int amount)
    {
        for (int i = 0; i < maxInventorySlots; i++)
        {
            if (inventory[i].itemName == itemName)
            {
                inventory[i].itemAmount += amount;
            }
        }
    }

    public void RemoveFromInventory(string itemName, int amount)
    {
        for (int i = 0; i < maxInventorySlots; i++)
        {
            if (inventory[i].itemName == itemName)
            {
                inventory[i].itemAmount -= amount;
            }
        }
    }
    
}
