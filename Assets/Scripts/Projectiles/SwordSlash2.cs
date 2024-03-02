using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash2 : Projectiles
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 10f; speed = 20f; delay = 1f;
        StartCoroutine(DestoryAfterDelay(delay));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
