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
        RaycastHit hit; // 카메라에서 쏜 레이저가 부딪힌 정보를 저장
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Input.mousePosition : 마우스의 화면 좌표값 구해줌, 카메라에서 마우스가 위치한 곳으로 가상의 선을 만듦

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // 부딪힌 게 있다면 참, 없다면 거짓 Physics.Raycast(ray(레이저를 쏨), out hit(부딪힌 정보), Mathf.Infinity(범위)
        {
            cursor.transform.position = new Vector3(hit.point.x, 0.2f, hit.point.z); // 부딪힌 위치로 커서 위치 저장, 위치값은 (변수.point)로
            // hit.collider.gameObject.name; 부딪힌 오브젝트의 이름을 가져옴

            if (Input.GetMouseButtonDown(0) && playerCtrl.playerState != PlayerState.Dead) // 마우스로 총알을 발사했을 때 캐릭터가 바라보는 방향을 커서 위치로
            {
                target.position = new Vector3(hit.point.x, 0, hit.point.z);
                playerCtrl.lookDirection =
                    target.position - playerCtrl.transform.position; // lookDirection : 바라봐야 할 방향 변수
                playerCtrl.LookUpdate(true);
                playerCtrl.StartCoroutine("Shot"); // 총알이 하나씩 발사
                
            }
        }
    }
}
