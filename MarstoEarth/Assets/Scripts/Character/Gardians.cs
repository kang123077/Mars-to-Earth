using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gardians : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public Transform caster;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);

        transform.position = caster.transform.position;
        transform.Rotate(0f, speed * Time.deltaTime, 0f); // y축 기준 회전
    }
}
