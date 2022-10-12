using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatformPM : MonoBehaviour
{
    private Transform lastParent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Platforma a atins ceva!");
        if (collision.gameObject.name.Equals("Player"))
        {
            lastParent = collision.gameObject.transform.parent;
            collision.gameObject.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       // Debug.Log("Platforma s-a detasat de ceva!");
        if (collision.gameObject.name.Equals("Player"))
        {
            collision.gameObject.transform.SetParent(lastParent);
        }
    }
    
}
