using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float power;
    public float life;


    private void Start()
    {
        Destroy(gameObject, life); // life만큼 시간이 흐르면 게임오브젝트 삭제
    }
    private void Update()
    {
        /*life -= Time.deltaTime; // 생성되고 삭제되기 까지의 시간
        if (life <= 0)
            Destroy(gameObject);*/

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag =="Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // 에네미 개체 가져옴

            if (enemy.enemyState != EnemyState.Die)
                enemy.Hurt(power); // 에네미 스크립트의 헐트 호출 후 파워값 리턴
        }

        Destroy(gameObject);
    }
}
