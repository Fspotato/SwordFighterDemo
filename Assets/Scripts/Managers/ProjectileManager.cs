using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] GameObject[] Projectiles;
    public void Sword_Attack(GameObject from)
    {
        GameObject SwordSlash = Instantiate(Projectiles[0]);
        SwordSlash.GetComponent<SwordSlash>()._from = LayerMask.LayerToName(from.layer);
        SwordSlash.GetComponent<SpriteRenderer>().flipX = from.GetComponent<SpriteRenderer>().flipX;
        SwordSlash.transform.position = from.transform.position;
    }
    public void Sword_Attack2(GameObject from)
    {
        GameObject SwordSlash = Instantiate(Projectiles[1]);
        SwordSlash.GetComponent<SwordSlash2>()._from = LayerMask.LayerToName(from.layer);
        SwordSlash.GetComponent<SpriteRenderer>().flipX = from.GetComponent<SpriteRenderer>().flipX;
        SwordSlash.transform.position = from.transform.position;
    }
}
