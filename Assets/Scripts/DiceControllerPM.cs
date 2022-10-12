using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControllerPM : MonoBehaviour
{
    
    [SerializeField] private DiceState currentState;
    private Animator animator;  
    // Start is called before the first frame update
    void Start()
    {
        currentState = (DiceState)Random.Range(1, 7);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator)
        {
            animator.SetInteger("DiceValue", (int)currentState);
        }
    }
    public int getValue()
    {
        return ((int)currentState);
    }

    public void Roll(bool random = true)
    {
        if(random == true)
        {
            currentState = (DiceState)Random.Range(1, 7);       
        }
        else
        {
            currentState = (DiceState)(((int)currentState) + 1);
            if (((int)currentState) > 6)
            {
                currentState = DiceState.One;
            }
        }
    }

    public enum DiceState
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six
    }
}
