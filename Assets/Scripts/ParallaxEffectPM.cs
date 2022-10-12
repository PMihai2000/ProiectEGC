using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffectPM : MonoBehaviour
{
    private float length, startPos;
    public Camera cam;
    public float parallaxEffect;
    private float maxDistanceUntilTranslation;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        maxDistanceUntilTranslation = cam.rect.width * cam.orthographicSize * 1.5f;
    }

    // Update is called once per frame 
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        float distCamVsObj = cam.transform.position.x - cam.transform.position.x;
        Vector3 objPos = transform.position;
        objPos.x = startPos + dist;
        transform.position = objPos;

        

        if (distCamVsObj >= maxDistanceUntilTranslation)
        {
            startPos += 2*length;
        }
        else if (distCamVsObj <= -maxDistanceUntilTranslation)
        {
            startPos -= 2 * length;
        }

    }
}
