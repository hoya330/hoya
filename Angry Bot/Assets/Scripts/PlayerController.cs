using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState // ������, ���� ����
{
    Idle,
    Walk,
    Run,
    Attack,
    Dead,
}
public class PlayerController : MonoBehaviour
{
    public PlayerState playerState; // ������, ���� �̸�
    public Vector3 lookDirection; // ������Ʈ�� �ٶ���� �ϴ� ����
    public float speed; // 0���� �� �������� �ʴٰ� walkSpeed or runSpeed ���� ���� Ű�� ���� ����
    public float walkSpeed;
    public float runSpeed;
    
    private Animation anim; // �̰��� ���� ���� �ִϸ��̼� ���� ������ 
    public AnimationClip idleAni;
    public AnimationClip walkAni;
    public AnimationClip runAni;

    private AudioSource audioSrc; 
    public AudioClip shotSound;
    public GameObject bullet;
    public Transform shotPoint; // �ѱ� ��ġ
    public GameObject shotFx;

    public Slider lifeBar;
    public float maxHp;
    public float hp;


    private void Start()
    {
        anim = GetComponent<Animation>();
        audioSrc = GetComponent<AudioSource>();
    }
    private void Update() // ������Ʈ �޼��� �κ��� �޼��� ȣ�⸸ �ִ� ���� ����
                          // ������Ʈ�� ������ Ŀ ������ �ʿ��� �� ����
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
                else // ����Ʈ�� ������ �ʾҴٸ�
                {
                    speed = walkSpeed;
                    playerState = PlayerState.Walk; // PlayerState �������� playerState�� Walk ������ �־��� 
                }
            }
            else if (playerState != PlayerState.Idle) // xx�� zz�� ���� ��� 0�� ��, ����Ű�� ������ �ִٰ� ���� ������ playerState ���� Idle�� �ƴ�
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
        Quaternion r = Quaternion.LookRotation(lookDirection); // Quaternion.LookRotation : �־��� ������ ���ϵ��� ��ü�� ȸ����Ű�� ���ʹϾ��� ����
        if (rightNow)
        {
            transform.rotation = r;
        }
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, r, 600f * Time.deltaTime); // Quaternion.RotateTowards: ��� ȸ�� �������� ��ü�� ȸ���մϴ�.

        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Translate(��ġ * �ӵ� * deltaTime)     
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
            GetComponent<Collider>()); // �Ѿ˰� �÷��̾��� �浹�� �����Ѵ�

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
