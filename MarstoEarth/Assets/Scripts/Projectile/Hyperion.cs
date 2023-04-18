using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class Hyperion : MonoBehaviour
    {
        private float speed;
        private float lifeTime;
        private float duration;
        private float range;
        private float dmg;
        private int layerMask;

        private Transform thisTransform;
        private Character.Character target;

        private readonly Collider[] colliders = new Collider[6];
        private float atkSpd;
        private static readonly Vector3[] ports = new Vector3[16]
        {
            new Vector3(.2f, .0f, .2f),
            new Vector3(.65f, .0f, .65f),
            new Vector3(.85f, .0f, .25f),
            new Vector3(.25f, .0f, .85f),
            new Vector3(-.2f, .0f, .2f),
            new Vector3(-.65f, .0f, .65f),
            new Vector3(-.85f, .0f, .25f),
            new Vector3(-.25f, .0f, .85f),
            new Vector3(.2f, .0f, -.2f),
            new Vector3(.65f, .0f, -.65f),
            new Vector3(.85f, .0f, -.25f),
            new Vector3(.25f, .0f, -.85f),
            new Vector3(-.2f, .0f, -.2f),
            new Vector3(-.65f, .0f, -.65f),
            new Vector3(-.85f, .0f, -.25f),
            new Vector3(-.25f, .0f, -.85f),
        };

        private Transform[] curPorts;

        private ProjectileInfo projectileInfo;
        private float eleapse;

        public void Init(int lm, float dg, float rg, float dr, float sp)
        {
            layerMask = lm;
            dmg = dg;
            range = rg;
            lifeTime = duration = dr;
            speed = sp;
            atkSpd = 10 *(1/ speed);
            thisTransform = transform;

            projectileInfo = new ProjectileInfo(layerMask,
                ResourceManager.Instance.projectileMesh[(int)Mesh.Bullet1].sharedMesh,
                Type.Bullet, (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(point, range * .2f, colliders, layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out target);
                        if (target)
                            target.Hit(point, dmg,0);
                    }
                });

            curPorts = new Transform[16];
            for (int i = 0; i < 16; i++)
            {
                GameObject port = new();
                port.transform.position = transform.position + range * ports[i];
                curPorts[i] = port.transform;
                port.transform.SetParent(transform);
            }
        }

        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime < 0)
                Destroy(gameObject);
            thisTransform.position += thisTransform.forward * (Time.deltaTime * speed * 0.2f);
            eleapse += Time.deltaTime;
            if (eleapse > atkSpd)
            {
                eleapse -= atkSpd;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 shotPoint = curPorts[i * 4 + UnityEngine.Random.Range(1, 5) - 1].position;
                    SpawnManager.Instance.Launch(shotPoint, Vector3.down, dmg, duration * 0.1f, speed*1.5f, range,
                        ref projectileInfo);
                }
            }


        }

    }
}