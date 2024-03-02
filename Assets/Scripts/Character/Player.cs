using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using States;

public class Player : MonoBehaviour
{
    [Header("控制鍵位")]
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode JumpKey;
    public KeyCode SwiftKey;
    public KeyCode DownKey;
    public KeyCode AttackKey;

    [Header("碰撞與鋼體")]
    Rigidbody2D rb; //鋼體

    [Header("角色數值")]
    public float maxhp; //總血量
    public float hp; //當前血量
    public List<PlayerBuffs> activeBuffs = new List<PlayerBuffs>(); //當前持有BUFF

    [Header("基礎數值")]
    float jump; //跳躍高度
    float speed; //移動速度
    float SwiftCooldown; //瞬移冷卻
    float Swiftdistance; //瞬移距離
    float downtime; //跳下平台所需時間

    [Header("動畫與視圖")]
    Animator anim; //動畫事件
    SpriteRenderer direction; //控制人物朝向

    [Header("音效")]
    AudioSource AttackSound; //攻擊音效
    AudioSource JumpSound; //跳躍音效
    AudioSource HurtSound; //受傷音效
    AudioSource SwiftSound; //瞬移音效
    AudioSource HealSound; //治癒音效
    public AudioSource ChangeSound; //攻擊變換音效

    [Header("其他")]
    string originalLayer; //初始圖層 用於穿梭平台時記錄原圖層
    bool isGrounded; //落地檢測
    bool down; //下跳檢測 
    bool _Attack; //攻擊檢測 
    public bool _Swift; //瞬移檢測
    public UnityEvent Dead; //死亡廣播
    public Slider HpBar; //血條

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); HpBar.value = 1f;
        jump = 6.5f; speed = 5f; hp = 50; maxhp = 50; Swiftdistance = 3f;
        downtime = 0.4f; SwiftCooldown = 0.5f;
        anim = GetComponent<Animator>(); direction = GetComponent<SpriteRenderer>(); AttackSound = GetComponent<AudioSource>();
        JumpSound = transform.Find("Jump").GetComponent<AudioSource>();
        HurtSound = transform.Find("Hurt").GetComponent<AudioSource>();
        SwiftSound = transform.Find("Swift").GetComponent<AudioSource>();
        HealSound = transform.Find("Heal").GetComponent<AudioSource>();
        ChangeSound = transform.Find("Change").GetComponent<AudioSource>();
        originalLayer = LayerMask.LayerToName(gameObject.layer);
        if (LeftKey == KeyCode.None)
        {
            LeftKey = KeyCode.A; RightKey = KeyCode.D; DownKey = KeyCode.S; JumpKey = KeyCode.W; SwiftKey = KeyCode.K; AttackKey = KeyCode.J;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        DownJump();
        Swift();
        Attack();
        if (activeBuffs.Contains(PlayerBuffs.AttackChange))
        {
            anim.SetBool("Change", true);
        }
        else
        {
            anim.SetBool("Change", false);
        }
    }

    void FixedUpdate()
    {
        isGroundedCheck();
    }

    void Attack() //攻擊
    {
        if (Input.GetKey(AttackKey) && (_Attack == false))
        {
            anim.SetTrigger("Attack"); AttackSound.Play();
            if (activeBuffs.Contains(PlayerBuffs.AttackChange))
            {
                transform.GetComponent<ProjectileManager>().Sword_Attack2(gameObject);
            }
            else
            {
                transform.GetComponent<ProjectileManager>().Sword_Attack(gameObject);
            }
            _Attack = true;
            StartCoroutine(AttackDelay());
        }
    }

    public void HpModify(HpModifers type, float value) //受到傷害
    {
        if (type == HpModifers.Attack)
        {
            hp -= value; HurtSound.Play(); anim.SetTrigger("Hurt");
        }
        else if (type == HpModifers.Heal)
        {
            hp += value; HealSound.Play();
            if (hp >= maxhp) hp = maxhp;
        }
        HpBar.value = hp / maxhp;
        if (hp <= 0) StartCoroutine(Die());
    }

    public IEnumerator Die() //死亡
    {
        yield return new WaitForSeconds(0.1f);
        Dead.Invoke();
        Destroy(gameObject);
    }

    void Move() //下左右移動兼平台穿越兼跳躍補充功能
    {
        //Debug.DrawLine(transform.position, transform.position + new Vector3(0.4f, 0f, 0f), Color.red);
        if (Input.GetKey(LeftKey))
        {
            //動畫及人物朝向
            direction.flipX = true;
            anim.SetBool("Run", true);
            //防撞左牆 實現方式：往左射出射線偵測前方是否有"Ground"
            var hits = Physics2D.RaycastAll(transform.position, Vector2.left, 0.35f);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    return;
                }
            }
            //向左移動
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(RightKey))
        {
            //動畫及人物朝向
            direction.flipX = false;
            anim.SetBool("Run", true);
            //防撞右牆 
            var hits = Physics2D.RaycastAll(transform.position, Vector2.right, 0.35f);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    return;
                }
            }
            //向右移動
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    void DownJump() //平台下跳
    {
        //需要設置角色y軸不動時才會觸發此功能 不然一樣會出現奇怪的操作
        if (Input.GetKey(DownKey) && rb.velocity.y == 0)
        {
            var hits = Physics2D.RaycastAll(transform.position + new Vector3(0f, -0.4f, 0f), Vector2.down, 0.3f);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Platform")
                {
                    gameObject.layer = LayerMask.NameToLayer("DownPlayer");
                    //當下跳時暫時移除平台穿越功能 否則會出現很奇怪的操作
                    down = true;
                    StartCoroutine(DownDelay(downtime));
                }
            }
        }
    }

    void isGroundedCheck() //防連跳補充跳躍法(不動時才偵測並補充跳躍次數)
    {
        if ((rb.velocity.y == 0) && (isGrounded == false))
        {
            var hits = Physics2D.RaycastAll(transform.position + new Vector3(0f, -0.4f, 0f), Vector2.down, 0.025f);
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Ground")
                {
                    isGrounded = true;
                }
                //平台實體化功能
                if (hit.collider.gameObject.tag == "Platform" && down == false)
                {
                    isGrounded = true;
                }
            }
        }
    }

    void Swift() //瞬移功能 實現方式：把角色的速度瞬間加速後降回本來的速度
    {
        if (Input.GetKey(SwiftKey) && (_Swift == false))
        {
            speed *= Swiftdistance;
            _Swift = true; SwiftSound.Play();
            StartCoroutine(SwiftDelay(SwiftCooldown));
        }
    }

    void Jump() //跳躍功能;
    {
        if (Input.GetKey(JumpKey) && isGrounded)
        {
            rb.velocity = new Vector2(0, jump); //把x設置成0 不然偶爾會有飄移的情形
            anim.SetTrigger("Jump"); JumpSound.Play();
            isGrounded = false;
        }
    }

    private IEnumerator DownDelay(float delay) //下跳延遲
    {
        yield return new WaitForSeconds(delay);
        gameObject.layer = LayerMask.NameToLayer(originalLayer);
        down = false;
    }

    private IEnumerator SwiftDelay(float delay) //瞬移冷卻
    {
        yield return new WaitForSeconds(0.1f); //瞬移持續時間
        speed = 5f;
        yield return new WaitForSeconds(delay);
        _Swift = false;
    }

    private IEnumerator AttackDelay() //攻擊冷卻
    {
        yield return new WaitForSeconds(0.75f); //攻擊冷卻
        _Attack = false;
    }

    //Buff及持續時間計數器
    Dictionary<PlayerBuffs, Coroutine> BuffTimers = new Dictionary<PlayerBuffs, Coroutine>();

    public void ApplyBuff(PlayerBuffs buff, float time) //增加Buff(buff,持續時間)
    {
        if (activeBuffs.Contains(buff))
        {
            if (BuffTimers.ContainsKey(buff))
            {
                StopCoroutine(BuffTimers[buff]);
                BuffTimers[buff] = StartCoroutine(BuffTimer(buff, time));
            }
        }
        else
        {
            BuffTimers[buff] = StartCoroutine(BuffTimer(buff, time));
        }
    }

    IEnumerator BuffTimer(PlayerBuffs buff, float time) //Buff(buff,持續時間)
    {
        if (!activeBuffs.Contains(buff))
        {
            activeBuffs.Add(buff);
        }
        yield return new WaitForSeconds(time);
        activeBuffs.Remove(buff);
        BuffTimers.Remove(buff);
    }
}
