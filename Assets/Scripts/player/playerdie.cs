using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerdie : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private item_collector ic;
    void Start()
    {
        ic = GetComponent<item_collector>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
