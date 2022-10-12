using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerPM : MonoBehaviour
{
    [SerializeField] private PlayerControllerPM player;
    [SerializeField] private Camera cam;
    [SerializeField] private float maxDistanceBetweenCamAndPlayer = 9f;
    [SerializeField] private float camFollowVelocity = 8f;
    [SerializeField] private float maxFallingDistanceUntilDeath = 50f;

    [SerializeField] private Text playerObjective;
    [SerializeField] private Text statusLevel;
    [SerializeField] private bool debugMode = false;
    
    [SerializeField] private GameObject[] firstZoneKeys;
    [SerializeField] private GateActivationPM firstZoneGateActivator;
    
    [SerializeField] private DiceControllerPM[] secondZoneCode;
    [SerializeField] private DiceControllerPM[] secondZoneCodeHints;
    [SerializeField] private GateActivationPM secondZoneGateActivator;

    [SerializeField] private int NextLevelIndex=-1;
    [SerializeField] private int ExitLevelIndex=-1;

    [SerializeField] private float timeUntilSceneEnds = 20.0f;

    [SerializeField] private AudioSource audioSource;
    public AudioClip bgmAudio;
    public AudioClip levelFailedAudio;
    public AudioClip levelCompleteAudio;
    private bool bgmMusicOn;

    // Start is called before the first frame update
    void Start()
    {
        //PORNIM MUZICA DE FUNDAL
        audioSource.PlayOneShot(bgmAudio);
        bgmMusicOn = true;

        ///INITIALIZARE CONFIGURATII INITIALE DIN PRIMA ZONA
        if (firstZoneKeys==null && firstZoneKeys.Length == 0)
        {
            Debug.Log("Keys from first zone non existent.");
        }

        ///INITIALIZARE CONFIGURATII INITIALE DIN A DOUA ZONA
        if(secondZoneCode == null || secondZoneCode.Length == 0)
        {
            Debug.Log("Second zone code doesn't exist.");
        }
        if(secondZoneCodeHints == null || secondZoneCodeHints.Length != secondZoneCode.Length)
        {
            Debug.Log("Second zone code hints doesn't match the length of the second zone main code!");
        }

        //Verificam ca mecanismul de setare al codului din
        //a doua zona sa nu fie deja setat pe codul corect 
        if (secondZoneCode != null && secondZoneCodeHints != null)
        {
            while (foundTheCodeFromSecondZone())
            {
                secondZoneCodeHints[Random.Range(0, secondZoneCodeHints.Length)].Roll();
            }
        }

        playerObjective.text = $"Player objective: find {firstZoneKeys.Length} keys to open the gate";
        player.Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -maxFallingDistanceUntilDeath)
        {       
            player.health.Decrement(10);
            player.Respawn();
        }

        if(debugMode == true)
        {
            ConsoleDebug();
        }

        if (foundAllKeysFromFirstZone())
        {
            if (!firstZoneGateActivator.isActivated())
            {
                firstZoneGateActivator.Locked = false;
            }
            else
            {
                playerObjective.text = "Current objective: find the code to unlock the next gate";
            }
        }
        if (foundTheCodeFromSecondZone() && !secondZoneGateActivator.isActivated())
        {
            secondZoneGateActivator.Locked = false;
            
        }
        if (!player.health.IsAlive)
        {
            if (bgmMusicOn)
            {
                bgmMusicOn = false;
                audioSource.PlayOneShot(levelFailedAudio);
            }
            

            player.controlEnabled = false;
            statusLevel.text = "GAME OVER!";
            playerObjective.text = "Press R to restart the level or Esc to leave the game...";
            if (Input.GetKeyUp(KeyCode.R))
            {
                SceneManager.LoadScene(NextLevelIndex - 1);
            }else
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                
                if (ExitLevelIndex < 0)
                {
                    Application.Quit();
                }
                else
                {
                    SceneManager.UnloadSceneAsync(NextLevelIndex-1);
                    SceneManager.LoadScene(ExitLevelIndex);
                }
            }
        }
        if (player.GoalReached)
        {
            if (bgmMusicOn)
            {
                bgmMusicOn = false;
                audioSource.PlayOneShot(levelCompleteAudio);
            }

            playerObjective.text = "Current objective: -";
            statusLevel.text = "LEVEL COMPLETE!";

            if (timeUntilSceneEnds >= 0)
            {
                timeUntilSceneEnds -= Time.deltaTime * 2;
            }else if(timeUntilSceneEnds < 0)
            {
                SceneManager.UnloadSceneAsync(NextLevelIndex - 1);
                SceneManager.LoadScene(NextLevelIndex);
            }
        }
    }

    

    private bool foundAllKeysFromFirstZone()
    {
        foreach(GameObject key in firstZoneKeys)
        {
            if (key.active)
            {
                return false;
            }
        }

        return true;
    }

    private bool foundTheCodeFromSecondZone()
    {
        for(int indexCode =0;indexCode < secondZoneCode.Length; indexCode++)
        {
            if (secondZoneCode[indexCode].getValue() != secondZoneCodeHints[indexCode].getValue())
            {
                return false;
            }
        }


        return true;
    }


   

    private void ConsoleDebug()
    {
        if (player.velocity.magnitude > 0)
        {
            //cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,-1);
            Debug.Log($"Player position: x:{player.transform.position.x} y:{player.transform.position.y}");
            Debug.Log($"Camera position: x:{cam.transform.position.x} y:{cam.transform.position.y}");
            
        }  
    }

}

