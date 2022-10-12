using System.Collections;
using UnityEngine;


public class DragableObjectPM : MonoBehaviour
{
    public bool isDragable = true;
    private Transform lastHolder;
    private Rigidbody2D rb2;
    private BoxCollider2D[] colliders;
    private bool itWasKinematic;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider2D>();
        rb2 = GetComponent<Rigidbody2D>();
        lastHolder = transform.parent;
        itWasKinematic = rb2.isKinematic;
    }

    public void Hold(GameObject holder)
    {
        lastHolder = this.transform.parent;
        this.transform.SetParent(holder.transform);
        rb2.isKinematic = true;
        foreach(Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void UnHold()
    {
        this.transform.SetParent(lastHolder);
        rb2.isKinematic = itWasKinematic;
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }
}

