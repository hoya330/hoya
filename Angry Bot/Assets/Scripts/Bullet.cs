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
        Destroy(gameObject, life); // life��ŭ �ð��� �帣�� ���ӿ�����Ʈ ����
    }
    private void Update()
    {
        /*life -= Time.deltaTime; // �����ǰ� �����Ǳ� ������ �ð�
        if (life <= 0)
            Destroy(gameObject);*/

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag =="Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>(); // ���׹� ��ü ������

            if (enemy.enemyState != EnemyState.Die)
                enemy.Hurt(power); // ���׹� ��ũ��Ʈ�� ��Ʈ ȣ�� �� �Ŀ��� ����
        }

        Destroy(gameObject);
    }
}
