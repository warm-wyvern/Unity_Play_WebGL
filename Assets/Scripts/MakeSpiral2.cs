using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpiral2 : MonoBehaviour
{
    // move this point in a spiral
    Vector3 spiralPoint;
    float Radius;
    float ExpandSpeed;
    float angle;
    float traveled;
    float speed;
    // place this object along the spiral
    GameObject spiralObject;
    int maxObjects;
    int objectCount;
    GameObject[] spiralObjects;
    // how long before the spiral cleans itself up and repeats itself
    float scriptEndTime;
    bool scriptEnding;
    // add z dimension
    int zDirection;
    Vector3 zMove;
    // constant
    float deltaTime;
    void Start()
    {
        // reposition at 0;
        transform.position = Vector3.zero;
        zDirection = Random.Range(0, 2); // 0||1 (max excl.)
        if (zDirection == 0) { zDirection = -1; }
        Debug.Log(zDirection);
        zMove = Vector3.forward;

        spiralPoint = transform.position;
        Radius = Random.Range(3f, 4f);
        ExpandSpeed = Random.Range(0.05f, 0.15f);
        angle = 0f;
        traveled = 0f;
        spiralObject = GameObject.Find("Spinning Cube");
        maxObjects = Random.Range(125, 150);
        objectCount = 0;
        spiralObjects = new GameObject[maxObjects+1];
        scriptEndTime = Time.time + Random.Range(10f, 15f);
        scriptEnding = false;
        // color stuff
        skip = Random.Range(1, 20);
        skip = maxObjects / skip;
        rgbSpeed = Random.Range(0.05f, 0.3f);
        rOffset = Random.Range(0, 3);
        switch (rOffset)
        {
            case 0: rOffset = 0; gOffset = 2; bOffset = 4; break;
            case 1: rOffset = 60; gOffset = 62; bOffset = 64; break;
            case 2: rOffset = 120; gOffset = 122; bOffset = 124; break;
            case 3: rOffset = 180; gOffset = 182; bOffset = 184; break;
        }
        width = Random.Range(128, 319);
        //
        deltaTime = Time.fixedDeltaTime;
    }

    // moving the spiral & placing objects done on fixed physics timestep,
    // "erasing" spiral happens 1 object per frame however fast it can (artistic effect)
    void Update()
    {
        if (scriptEnding)
        {
            if(objectCount >= 1)
            {
                --objectCount;
                Destroy(spiralObjects[objectCount]);
            }
            else
            {
                gameObject.AddComponent<MakeSpiral2>();
                Destroy(this);
                // done cleaning up objects, destroy this instance (create another causing it to repeat)
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time >= scriptEndTime)
        {
            scriptEnding = true;
        }
        else
        {
            SpiralPoint(); // move the point in a spiral
        }
    }

    void SpiralPoint()
    {
        float radPI = Radius * Mathf.PI;
        speed = radPI / Radius;
        Radius += radPI * ExpandSpeed * Time.fixedDeltaTime;

        angle += speed * Time.fixedDeltaTime;
        traveled += speed * Time.fixedDeltaTime;

        // it might be a little late, but place a thingy so often (11.25 degrees)
        if (traveled >= 0.19634954f)
        {
            traveled = 0f;
            if (objectCount < maxObjects)
            {
                GameObject go = Instantiate(spiralObject,
                    spiralPoint, Quaternion.identity);

                SetColor(go.GetComponent<Renderer>()); // make annoying rainbows

                // refence objects for later cleanup
                spiralObjects[objectCount] = go;
                ++objectCount;
            }
        }

        float z = spiralPoint.z;
        spiralPoint = new Vector3(
            Mathf.Cos(angle) * Radius,
            Mathf.Sin(angle) * Radius,
            z + (zDirection * speed * deltaTime));
    }

    int i = 0;
    int skip;
    float rgbSpeed;
    int width;
    int brightness = 255;
    int rOffset;
    int gOffset;
    int bOffset;
    bool flippityfloppity = true;
    void SetColor(Renderer renderer)
    {
        // rolling value:
        if (flippityfloppity)
        {
            ++i;
            if (i >= skip) { flippityfloppity = false;}
        }
        else
        {
            --i;
            if (i <= 0) { flippityfloppity = true;}
        }

        float red = Mathf.Sin(rgbSpeed * i + rOffset) * width + brightness;
        float green = Mathf.Sin(rgbSpeed * i + gOffset) * width + brightness;
        float blue = Mathf.Sin(rgbSpeed * i + bOffset) * width + brightness;
        Color rot = new Color(red / 255, green / 255, blue / 255);

        MaterialPropertyBlock m = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(m);
        m.SetColor("_Color", rot);
        renderer.SetPropertyBlock(m);
    }

    void DrawDebug()
    {
        Debug.DrawRay(spiralPoint, Vector3.up * 3f, Color.magenta, 10f);
        Debug.DrawRay(spiralPoint, Vector3.down * 3f, Color.cyan, 10f);
    }
}
