using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseContllor : MonoBehaviour
{
    public Transform target;
    public Transform cursor;
    public PlayerController playerCtrl;


    private void Update()
    {
        RaycastHit hit; // ī�޶󿡼� �� �������� �ε��� ������ ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Input.mousePosition : ���콺�� ȭ�� ��ǥ�� ������, ī�޶󿡼� ���콺�� ��ġ�� ������ ������ ���� ����

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // �ε��� �� �ִٸ� ��, ���ٸ� ���� Physics.Raycast(ray(�������� ��), out hit(�ε��� ����), Mathf.Infinity(����)
        {
            cursor.transform.position = new Vector3(hit.point.x, 0.2f, hit.point.z); // �ε��� ��ġ�� Ŀ�� ��ġ ����, ��ġ���� (����.point)��
            // hit.collider.gameObject.name; �ε��� ������Ʈ�� �̸��� ������

            if (Input.GetMouseButtonDown(0) && playerCtrl.playerState != PlayerState.Dead) // ���콺�� �Ѿ��� �߻����� �� ĳ���Ͱ� �ٶ󺸴� ������ Ŀ�� ��ġ��
            {
                target.position = new Vector3(hit.point.x, 0, hit.point.z);
                playerCtrl.lookDirection =
                    target.position - playerCtrl.transform.position; // lookDirection : �ٶ���� �� ���� ����
                playerCtrl.LookUpdate(true);
                playerCtrl.StartCoroutine("Shot"); // �Ѿ��� �ϳ��� �߻�
                
            }
        }
    }
}
