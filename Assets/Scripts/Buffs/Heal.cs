using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

public class Heal : Buffs
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterDelay(15f));
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().HpModify(HpModifers.Heal, 10f);
            Destroy(gameObject);
        }
    }
}
