using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateActivationPM : MonoBehaviour
{
    
    [SerializeField] private ObjectFallAndDestroyPM gate;
    [SerializeField] private AudioSource audioSource;
    public AudioClip unlockedAudio;
    public AudioClip lockedAudio;
    // Start is called before the first frame update
    public bool Locked { get; set; } = true;
    private bool activated = false;

    public bool isActivated()
    {
        return activated;
    }
    

    //Just a demo... i guess :))
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name.Equals("Player"))
        {
            if (!Locked)
            {
                //Debug.Log("Player-ul a activat mecanismul primei porti!");
                audioSource.PlayOneShot(unlockedAudio);
                gate.Activate();
                activated = true;
            }
            else
            {
                audioSource.PlayOneShot(lockedAudio);
            }
        }
    }
}
