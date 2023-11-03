using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Die,
}
public class Enemy : MonoBehaviour
{
    public EnemyState enemyState;

    public Animator anim;

    private float speed;
    public float moveSpeed;
    public float attackSpeed;

    public float findRange; // 에너미가 추격할 범위
    public float damage;
    public Transform player;

    private AudioSource audioSrc;
    public Transform fxPoint; // 이펙트 위치
    public GameObject hitFx; // 이펙트 위치 저장
    public AudioClip hitSound; // AudioClip - 사운드 파일 
    public AudioClip deathSound;

    public GameObject guiPivot; // 캔버스 저장, 에너미 죽고나서 끄기 위함
    public Slider lifeBar;
    public float maxHp;
    public float hp;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (enemyState == EnemyState.Idle)
            DistanceCheck();
        else if (enemyState == EnemyState.Move)
        {
            MoveUpdate();
            AttackRangeCheck();
        }
    }
    private void DistanceCheck()
    {
        if (Vector3.Distance(player.position, transform.position) >= findRange) // Vector3.Distance(플레이어 포지션, 에너미 포지션) -> 플레이어와 에너미 사이의 거리를 구해줌
        {
            if (enemyState != EnemyState.Idle)
            {
                enemyState = EnemyState.Idle;
                anim.SetBool("run", false);
                speed = 0;

            }
        }
        else
        {
            if (enemyState != EnemyState.Move)
            {
                enemyState = EnemyState.Move;
                anim.SetBool("run", true);
                speed = moveSpeed;
            }
        }
    }

    private void AttackRangeCheck()
    {
        if (Vector3.Distance(player.position, transform.position) < 1.5f &&
            enemyState != EnemyState.Attack)
        {
            speed = 0;
            enemyState = EnemyState.Attack;
            anim.SetTrigger("attack");
        }
    }
    private void MoveUpdate()
    {
        Vector3 dir = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position; // y축을 플레이어 y로 해주면 벡터가 대각선으로 만들어져 공중으로 이동할 수 있음
        transform.rotation = Quaternion.LookRotation(dir);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void AttackOn()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.Hurt(damage);
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            enemyState = EnemyState.Hurt;
            speed = 0;
            anim.SetTrigger("hurt");

            Instantiate(hitFx, fxPoint.position,
                Quaternion.LookRotation(fxPoint.forward));

            hp -= damage;
            lifeBar.value = hp / maxHp;

            audioSrc.clip = hitSound;
            audioSrc.Play();
        }

        if (hp <= 0)
            Death();
    }

    public void Death()
    {
        enemyState = EnemyState.Die;
        anim.SetTrigger("die");
        speed = 0;

        guiPivot.SetActive(false);
        audioSrc.clip = deathSound;
        audioSrc.Play();
    }
}