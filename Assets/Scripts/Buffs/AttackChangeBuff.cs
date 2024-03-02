using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

public class AttackChangeBuff : Buffs
{
    void Start()
    {
        buff = PlayerBuffs.AttackChange; time = 7.5f;
        StartCoroutine(DestroyAfterDelay(15f));
    }
    void Update()
    {
        if (transform.position.y >= 6f) transform.Translate(0f, -0.75f * Time.deltaTime, 0f);
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().ApplyBuff(buff, time);
            other.GetComponent<Player>().ChangeSound.Play();
            Destroy(gameObject);
        }
    }
}
