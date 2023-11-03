using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFX : MonoBehaviour
{
    public Light gunLight;


    private void Update()
    {
        gunLight.range = Random.Range(4f, 10f); // 총알 발사광의 범위
        transform.localScale = Vector3.one * Random.Range(2f, 4f);// Vector3.one = (1, 1, 1), 크기
        transform.localEulerAngles =
            new Vector3(270f, 0, Random.Range(0, 90f)); // 각도
    }

}
