using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Buffs;
    void Start()
    {
        StartCoroutine(ChangeAttackBuff());
        StartCoroutine(Heal());
    }
    IEnumerator ChangeAttackBuff() //產生攻擊型態變換Buff
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            GameObject Buff = Instantiate(Buffs[0]);
            Buff.transform.position = new Vector3(11.5f, 12.6f, 0f);
        }
    }
    IEnumerator Heal() //產生回血Buff
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            GameObject Buff = Instantiate(Buffs[1]);
            Buff.transform.position = new Vector3(3f, 8.5f, 0f);
            GameObject Buff2 = Instantiate(Buffs[1]);
            Buff2.transform.position = new Vector3(20f, 8.5f, 0f);
        }
    }
}
