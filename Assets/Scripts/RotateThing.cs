using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThing : MonoBehaviour
{
    public Vector3 Speed = new Vector3(5.625f, 11.25f, 22.5f);

    void Update()
    {
        Vector3 delta = Speed * Time.deltaTime;
        transform.Rotate(delta);
    }
}
