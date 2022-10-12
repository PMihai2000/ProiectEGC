using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectPM : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    // Start is called before the first frame update
    public float velocity = 1f;
    private int currentWaypointIndex = 0;

    private float velocityMultiplier = 100f;

    private Rigidbody2D rb2;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }

            if (rb2 == null)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, velocity * Time.deltaTime);
            }
            else
            {
                Vector3 v2Velocity = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, velocity * Time.deltaTime) - (new Vector2(transform.position.x,transform.position.y));

               
                rb2.velocity = v2Velocity*velocity*velocityMultiplier;
            }
        }
    }

}
