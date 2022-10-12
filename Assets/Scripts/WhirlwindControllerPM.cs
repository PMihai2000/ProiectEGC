using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindControllerPM : MonoBehaviour
{
    public float launchPower = 10f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag.Equals("Player"))
        {
            animator.SetTrigger("isNearby");
        }
    }
}
