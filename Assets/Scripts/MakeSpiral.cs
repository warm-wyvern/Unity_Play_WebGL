using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpiral : MonoBehaviour
{
    public GameObject InstanceThing; // instantiate this thing in a spiral pattern
    public int InstanceCount = 100;

    public float Radius = 5f;
    public float RadiusExpandSpeed = 0.1f;
    //public float speed = (2 * Mathf.PI) / 5f; // 5s to complete circle
    float speed;
    float angle = 0f;
    bool once = true;

    void Start()
    {

    }

    void Update()
    {
        Radius += RadiusExpandSpeed * Time.deltaTime;
        speed = (2 * Mathf.PI) / Radius;

        angle += speed * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Cos(angle) * Radius,
            Mathf.Sin(angle) * Radius, 0f);

        float euler = ((180 / Mathf.PI) * angle) % 11.25f;

        // window of opportunity, do-once latch
        if(euler >= 1f)
        {
            once = true;
        }
        else
        {
            if (once)
            {
                Instantiate(InstanceThing, transform.position, Quaternion.identity);
                --InstanceCount;
                if(InstanceCount < 0)
                {
                    Destroy(this);
                }
            }
            once = false;
        }
    }
}
