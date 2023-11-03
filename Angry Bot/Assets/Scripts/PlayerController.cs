using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState // 열거형, 변수 내용
{
    Idle,
    Walk,
    Run,
    Attack,
    Dead,
}
public class PlayerController : MonoBehaviour
{
    public PlayerState playerState; // 열거형, 변수 이름
    public Vector3 lookDirection; // 오브젝트가 바라봐야 하는 방향
    public float speed; // 0으로 둬 움직이지 않다가 walkSpeed or runSpeed 값을 누른 키에 따라 저장
    public float walkSpeed;
    public float runSpeed;
    
    private Animation anim; // 이곳에 밑의 동작 애니메이션 들을 저장함 
    public AnimationClip idleAni;
    public AnimationClip walkAni;
    public AnimationClip runAni;

    private AudioSource audioSrc; 
    public AudioClip shotSound;
    public GameObject bullet;
    public Transform shotPoint; // 총구 위치
    public GameObject shotFx;

    public Slider lifeBar;
    public float maxHp;
    public float hp;


    private void Start()
    {
        anim = GetComponent<Animation>();
        audioSrc = GetComponent<AudioSource>();
    }
    private void Update() // 업데이트 메서드 부분은 메서드 호출만 넣는 것이 좋음
                          // 업데이트의 비중이 커 정리가 필요할 수 있음
    {
        if (playerState != PlayerState.Dead)
        {
            KeyBoardInput();
            LookUpdate(false);      
        }
        AnimationUpdate();
    }

    void KeyBoardInput()
    {
        float xx = Input.GetAxis("Horizontal");
        float zz = Input.GetAxis("Vertical");

        if (playerState != PlayerState.Attack)
        {


            if (xx != 0 || zz != 0)
            {
                lookDirection = (xx * Vector3.right) + (zz * Vector3.forward); // (1, 0, 0) + (0, 0, 1) = (1, 0, 1)

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    speed = runSpeed;
                    playerState = PlayerState.Run;
                }
                else // 쉬프트를 누르지 않았다면
                {
                    speed = walkSpeed;
                    playerState = PlayerState.Walk; // PlayerState 열거형의 playerState에 Walk 변수를 넣어줌 
                }
            }
            else if (playerState != PlayerState.Idle) // xx와 zz의 값이 모두 0일 때, 방향키를 누르고 있다가 떼는 순간엔 playerState 값이 Idle이 아님
            {
                playerState = PlayerState.Idle;
                speed = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && playerState != PlayerState.Dead)
            StartCoroutine(nameof(Shot));
    }

    public void LookUpdate(bool rightNow)
    {
        Quaternion r = Quaternion.LookRotation(lookDirection); // Quaternion.LookRotation : 주어진 방향을 향하도록 객체를 회전시키는 쿼터니언을 생성
        if (rightNow)
        {
            transform.rotation = r;
        }
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 600f * Time.deltaTime); // Quaternion.RotateTowards: 대상 회전 방향으로 개체를 회전합니다.

        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Translate(위치 * 속도 * deltaTime)     
    }

       
    void AnimationUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                anim.CrossFade(idleAni.name, 0.2f);
                break;
            case PlayerState.Walk:
                anim.CrossFade(walkAni.name, 0.2f);
                break;
            case PlayerState.Run:
                anim.CrossFade(runAni.name, 0.2f);
                break;
            case PlayerState.Attack:
                anim.CrossFade(idleAni.name, 0.2f);
                break;
            case PlayerState.Dead:
                anim.CrossFade(idleAni.name, 0.2f);
                break;
        }
    }

    public IEnumerator Shot()
    {
        GameObject bulletObj = Instantiate(
            bullet,
            shotPoint.position,
            Quaternion.LookRotation(shotPoint.forward));

        Physics.IgnoreCollision(
            bulletObj.GetComponent<Collider>(),
            GetComponent<Collider>()); // 총알과 플레이어의 충돌을 무시한다

        audioSrc.clip = shotSound;
        audioSrc.Play();

        shotFx.SetActive(true);

        playerState = PlayerState.Attack;

        speed = 0;

        yield return new WaitForSeconds(0.15f);
        shotFx.SetActive(false);

        yield return new WaitForSeconds(0.15f);
        playerState = PlayerState.Idle;
    }


    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            lifeBar.value = hp / maxHp;
        }

        if (hp <= 0)
        {
            speed = 0;
            playerState = PlayerState.Dead;
        }
    }
}
