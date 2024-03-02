using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

public class Projectiles : MonoBehaviour
{
    public float damage; //投射物傷害
    public float speed; //投射物移動速度
    public string _from; //是誰投射的
    public float delay; //最長發射時間

    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (transform.GetComponent<SpriteRenderer>().flipX)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((LayerMask.LayerToName(other.gameObject.layer) != _from) && (other.gameObject.tag == "Player"))
        {
            if (other.GetComponent<Player>()._Swift == false)
            {
                other.GetComponent<Player>().HpModify(HpModifers.Attack, damage);
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator DestoryAfterDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        Destroy(gameObject);
    }
}
