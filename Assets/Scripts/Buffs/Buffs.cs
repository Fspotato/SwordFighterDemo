using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using States;
public class Buffs : MonoBehaviour
{
    public PlayerBuffs buff;
    public float time;
    public IEnumerator DestroyAfterDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        Destroy(gameObject);
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().ApplyBuff(buff, time);
            Destroy(gameObject);
        }
    }
}
