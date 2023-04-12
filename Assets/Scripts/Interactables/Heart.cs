using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public GameSignal powerUpSignal;
    public FloatValue playerHealth;
    public FloatValue heartContainers;
    public float amountToIncrease;
    Player player = Player.GetInstance();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            player.Heal(amountToIncrease);
            powerUpSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
