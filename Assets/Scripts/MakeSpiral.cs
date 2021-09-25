using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpiral : MonoBehaviour
{
    GameObject InstanceThing; // instantiate this thing in a spiral pattern
    public int InstanceCount = 100;
    public float SpeedFactor = 3f;

    public float Radius = 2f;
    public float RadiusExpandSpeed = 0.33f;
    //public float speed = (2 * Mathf.PI) / 5f; // 5s to complete circle
    float speed;
    float angle = 0f;
    bool once = true;

    // its fun to watch, so make it do this more than once
    // delete all of the cubes and instance a new one of these where it started when destroyed
    List<GameObject> cubes;
    Vector3 startPosition;

    float repeatFrequency = 15f;
    float repeatTime;
    public GameObject thisThing;

    bool cleanup = false;

    void Start()
    {
        repeatFrequency = Random.Range(14f, 16f);
        cubes = new List<GameObject>();
        cleanup = false;
        InstanceCount = Random.Range(75, 125);
        SpeedFactor = Random.Range(2f, 4f);
        Radius = Random.Range(1.5f, 3f);
        RadiusExpandSpeed = Random.Range(0.25f, 0.4f);
        angle = 0f;
        once = true;
        startPosition = transform.position;
        // just grab the instancething by name
        InstanceThing = GameObject.Find("Spinning Cube");
        repeatTime = Time.time + repeatFrequency;
        gameObject.name = "FOOEY";


    }

    void Update()
    {
        if(cleanup)
        {
            if(cubes.Count == 0)
            {
                Destroy(gameObject);
                return;
            }
            GameObject go = cubes[cubes.Count-1];
            cubes.RemoveAt(cubes.Count-1);
            Destroy(go);
            return;
        }

        RadiusExpandSpeed += Time.deltaTime * 0.1f;
        Radius += RadiusExpandSpeed * Time.deltaTime;
        speed = (SpeedFactor * Mathf.PI) / Radius;

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
                if (InstanceCount < 0)
                {
                    if (Time.time >= repeatFrequency)
                    {
                        DoAgain();
                    }
                }
                else
                {
                    GameObject go = Instantiate(InstanceThing, transform.position, Quaternion.identity);
                    cubes.Add(go);
                    SetColor(go.GetComponent<Renderer>());
                    --InstanceCount;
                }
            }
            once = false;
        }
    }

    int i = 0;
    int iterations = 100;
    float frequency = 0.3f;
    void SetColor(Renderer renderer)
    {
        ++i; // rolling value
        if(i >= iterations) { i = 0; }
        float red = Mathf.Sin(frequency * i + 0) * 63 + 128;
        float green = Mathf.Sin(frequency * i + 2) * 63 + 128;
        float blue = Mathf.Sin(frequency * i + 4) * 63 + 128;

        //Color rand = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Color rot = new Color(255 / red, 255 / green, 255 / blue);

        MaterialPropertyBlock m = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(m);
        m.SetColor("_Color", rot);
        renderer.SetPropertyBlock(m);
    }

    // delete all the cubes and this script, instantiate this script as a prefab again
    void DoAgain()
    {
        if(cleanup == false)
        {
            Instantiate(thisThing, startPosition, Quaternion.identity);
        }
        cleanup = true;
    }
}
