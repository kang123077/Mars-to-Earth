
using System;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

namespace Projectile
{
    public enum Mesh
    {
        Bullet1,
        Grenade
    }
    public enum Type
    {
        Bullet,
        Cannon,
        Satellite
    }

    public class Projectile:MonoBehaviour
    {
        public Transform attacker;
        public LayerMask layerMask; //안부딪힐 오브젝트 태그
        public float dmg;
        public MeshFilter mesh;

        private float lifeTime ; 
        private float speed; 
        private readonly Collider[] colliders = new Collider[5];
        
        public Vector3 targetPosition;
        public Type type;
        public float journeyTime = 1.0f;
        private float startTime;
        Transform thisTransform;
        private Vector3 orbitAxis;

        public void Init()
        {
            transform.position = attacker.position+ new Vector3(0,1f,0.5f);
            lifeTime = 2;
            
            transform.forward = targetPosition;
            speed = 40;
            startTime = Time.time;

            orbitAxis = (transform.position - attacker.position).normalized;
        }
        void Bullet()
        {
            thisTransform.position += thisTransform.forward * (Time.deltaTime * speed);

            if (Physics.OverlapSphereNonAlloc(thisTransform.position, 0.2f, colliders,
                    layerMask) > 0)
            {
                colliders[0].TryGetComponent(out Character.Character target);
                if (target)
                    target.Hit(attacker.position, dmg);
                SpawnManager.Instance.projectileManagedPool.Release(this);
            }
        }
        void Cannon()
        {
            Vector3 center = (attacker.position + targetPosition) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 riseRelCenter = attacker.position - center;
            Vector3 setRelCenter = targetPosition - center;
            float fracComplete = (Time.time - startTime) / journeyTime;
            thisTransform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            thisTransform.position += center;
            if (fracComplete > 1)
            {
                int count = Physics.OverlapSphereNonAlloc(thisTransform.position, 5, colliders,
                    layerMask);
                for (int i = 0; i < count; i++)
                {
                    colliders[i].TryGetComponent(out Character.Character target);
                    if (target)
                        target.Hit(attacker.position, dmg);
                }
                SpawnManager.Instance.projectileManagedPool.Release(this);
            }
        }
        void Satellite()
        {
            transform.RotateAround(attacker.position, Vector3.up, speed * Time.deltaTime);


        }


        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime<0)
                SpawnManager.Instance.projectileManagedPool.Release(this);
            thisTransform = transform;

            switch (type)
            {
                case Type.Bullet: Bullet(); break;
                case Type.Cannon: Cannon(); break;
                case Type.Satellite: Satellite(); break;
            }
        }
    }
}