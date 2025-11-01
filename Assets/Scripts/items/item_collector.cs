using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class item_collector : MonoBehaviour
{
    private BetterJumpController playerJumpController;
     void Start()
    {
        playerJumpController = GetComponent<BetterJumpController>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("orange"))
        {
            Destroy(collision.gameObject);
            playerJumpController.setcurrentJumpCount();
        }
    }
}
