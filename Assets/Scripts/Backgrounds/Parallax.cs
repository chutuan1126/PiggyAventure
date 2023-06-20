using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPosition;
    public new GameObject camera;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float dist = camera.transform.position.x * parallaxEffect;
        float temp = camera.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

        if (temp > startPosition + length) startPosition += length;
        else if (temp < startPosition - length) startPosition -= length;
    }
}
