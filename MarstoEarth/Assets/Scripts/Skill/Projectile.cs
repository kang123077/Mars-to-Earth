
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Skill
{
    public enum ProjectileType
    {
        Bullet1
    }
    public class Projectile:MonoBehaviour
    {
        public Vector3 attackerPosition;
        public LayerMask layerMask; //안부딪힐 오브젝트 태그
        public float dmg;
        private float lifeTime ; // 생존시간
        private float speed; 
        private readonly Collider[] colliders = new Collider[5];
        
        public Vector3 targetPosition;
        public bool isBullet;
        public float journeyTime = 1.0f;
        private float startTime;


        private void OnEnable()
        {
            transform.position = attackerPosition+ new Vector3(0,1f,0.5f);
            lifeTime = 2;
            if (isBullet)
            {
                transform.forward = targetPosition;
                speed = 35;
            }
            else
            {
                speed = 15;
                startTime = Time.time;
            }
                
        }

        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime<0)
                SpawnManager.Instance.projectileManagedPool.Release(this);
            Transform thisTransform = transform;
            
            if (isBullet)
            {
                thisTransform.position+= thisTransform.forward*(Time.deltaTime*speed);
                
                if (Physics.OverlapSphereNonAlloc(thisTransform.position, 0.2f, colliders,
                        layerMask) > 0)
                {
                    colliders[0].TryGetComponent(out Character.Character target);
                    if(target)
                        target.Hit(attackerPosition,dmg);
                    SpawnManager.Instance.projectileManagedPool.Release(this);
                }
            }
            else
            {
                Vector3 center = (attackerPosition + targetPosition) * 0.5F;

                center -= new Vector3(0, 1, 0);

                Vector3 riseRelCenter = attackerPosition - center;
                Vector3 setRelCenter = targetPosition - center;

                float fracComplete = (Time.time - startTime) / journeyTime;

                thisTransform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
                thisTransform.position += center;
                if (fracComplete > 1)
                {
                    int count = Physics.OverlapSphereNonAlloc(thisTransform.position, 3, colliders,
                        layerMask);
                    for(int i = 0; i<count; i++)
                    {
                        colliders[i].TryGetComponent(out Character.Character target);
                        if(target)
                            target.Hit(attackerPosition,dmg);
                        SpawnManager.Instance.projectileManagedPool.Release(this);
                    }
                }
                    
            }
            
            
            
        }
    }
}