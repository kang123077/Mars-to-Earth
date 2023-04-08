using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gardians : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    private Transform caster;

    // Update is called once per frame
    public void Init(Transform ct, float lf, float sp)
    {
        caster = ct;
        lifeTime = lf;
        speed = sp;
    }
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
        if (!caster)
            Destroy(gameObject);
        transform.position = caster.transform.position;
        transform.Rotate(0f, speed*10 * Time.deltaTime, 0f); // y축 기준 회전
    }
}
