using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartGamePM : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private int indexStartScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        
    }
    
    void TaskOnClick()
    {
        SceneManager.LoadScene(indexStartScene);
        
    }


}
