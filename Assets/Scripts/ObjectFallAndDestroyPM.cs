using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallAndDestroyPM : MonoBehaviour
{
    private Rigidbody2D rb2;
    [SerializeField] private float fallingDistance = 50f;

    private bool activated = false;

    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && rb2.isKinematic)
        {
            rb2.isKinematic = false;
            rb2.velocity = Vector2.down * 2;

            if (collider)
            {
                collider.enabled = false;
            }
            

            
        }

        if (activated && transform.position.y<= -fallingDistance)
        {
            Destroy(this.gameObject.transform.parent.parent.gameObject);
        }
    }
    public void Activate()
    {
        activated = true;
    }
}
