using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : MonoBehaviour
{
    public GameSignal context;
    public bool playerInRange;
    public Item contents;
    public Inventory playerInventory;
    public bool isOpen;
    public GameSignal raiseItem;
    public GameObject dialogBox;
    public Text dialogText;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (!isOpen)
            {
                // Open the chest
                OpenChest();
            }
            else
            {
                // Chest Is Already Open
                ChestAlreadyOpen();
            }
        }
    }

    public void OpenChest()
    {
        // Dialog Box On
        dialogBox.SetActive(true);

        // Dialog Text = contents text
        dialogText.text = contents.itemDescription;

        // Add contents to inventory
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;

        // Raise the signal to the player to animate
        raiseItem.Raise();

        // raise teh context clue
        context.Raise();

        // set the chest to opened
        isOpen = true;

        anim.SetBool("opened", true);
    }

    public void ChestAlreadyOpen()
    {
        // Dialog Box off
        dialogBox.SetActive(false);

        // raise the signal to the player to stop animation
        raiseItem.Raise();

        playerInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }

}
