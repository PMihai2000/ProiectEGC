using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScriptPM : MonoBehaviour
{
    public PlayerControllerPM player;
    private float initScale;
    
    
    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localScale = transform.localScale;
        localScale.x = ((float)player.health.currentHP / player.health.maxHP) * initScale;
        transform.localScale = localScale;
            
    }
}
