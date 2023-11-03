using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFX : MonoBehaviour
{
    public Light gunLight;


    private void Update()
    {
        gunLight.range = Random.Range(4f, 10f); // �Ѿ� �߻籤�� ����
        transform.localScale = Vector3.one * Random.Range(2f, 4f);// Vector3.one = (1, 1, 1), ũ��
        transform.localEulerAngles =
            new Vector3(270f, 0, Random.Range(0, 90f)); // ����
    }

}
