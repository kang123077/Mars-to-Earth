
using System;
using UnityEngine;

namespace Skill
{
    
    public class Projectile:MonoBehaviour
    {
        public Transform attacker;
        public LayerMask layerMask; //안부딪힐 오브젝트 태그
        public float dmg;
        private float lifeTime = 3; // 생존시간
        [SerializeField] private float speed = 25; // 탄속
        private readonly Collider[] colliders = new Collider[1];

        private void OnEnable()
        {
            transform.position =attacker.position+ new Vector3(0,0.5f,0.5f);
            transform.forward = attacker.forward;
            lifeTime = 3;
        }

       

        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime<0)
                SpawnManager.Instance.projectileManagedPool.Release(this);
            Transform thisTransform = transform;
            thisTransform.position+= thisTransform.forward*(Time.deltaTime*speed);
            
            if (Physics.OverlapSphereNonAlloc(thisTransform.position, 0.2f, colliders,
                    layerMask) > 0)
            {
                
                colliders[0].TryGetComponent(out Character.Character target);
                if(target)
                    target.Hit(attacker,dmg);
                SpawnManager.Instance.projectileManagedPool.Release(this);
            }

        }
    }
}