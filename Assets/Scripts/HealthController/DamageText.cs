using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float timeDestroy = 1f;

    private void Update()
    {
        timeDestroy -= Time.deltaTime;

        transform.position = new Vector3(transform.position.x + (Time.deltaTime / 3), transform.position.y + (Time.deltaTime / 2));

        if (timeDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
