
using System;
using UnityEngine;

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
    }
    public struct ProjectileInfo
    {
        public int lm;//layerMask
        public UnityEngine.Mesh ms;
        public Type ty;
        public Action<Vector3> ef;
        public ProjectileInfo(int layerMask, UnityEngine.Mesh mesh, Type type,Action<Vector3> effect)
        {   
            lm = layerMask;
            ms = mesh;
            ty = type;
            ef = effect;
        }
    }
    public class Projectile:MonoBehaviour
    {
        public Vector3 attackerPos;
        public Vector3 targetPos;
        public float dmg;
        public float duration;
        public float speed;
        public float range;

        public MeshFilter mesh;
        readonly private ProjectileInfo[] thisInfo = new ProjectileInfo[1];

        private readonly Collider[] colliders = new Collider[5];
        private Character.Character target;
        public float startTime;
        Transform thisTransform;
        public void Init(Vector3 ap, Vector3 tp, float dg, float dr, float sp , float rg ,ref ProjectileInfo info)
        {
            attackerPos = ap;
            targetPos = tp;
            dmg= dg;
            duration = dr;
            speed = sp;
            range = rg;

            transform.localScale = range*0.1f*Vector3.one;
            transform.position = ap + tp + new Vector3(0, 1f, 0.5f);
            transform.forward = tp;
            
            mesh.mesh = info.ms;
            thisInfo[0] = info;
            startTime = Time.time;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Bullet()
        {
            thisTransform.position += targetPos * (Time.deltaTime * speed); 
            if (Physics.OverlapSphereNonAlloc(thisTransform.position, range, colliders,
                    thisInfo[0].lm) > 0)
            {
                colliders[0].TryGetComponent(out target);
                if (target)
                    target.Hit(attackerPos, dmg);
                thisInfo[0].ef?.Invoke(thisTransform.position);
                SpawnManager.Instance.projectileManagedPool.Release(this);
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void Cannon()
        {
            Vector3 center = (attackerPos + targetPos) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 riseRelCenter = attackerPos - center;
            Vector3 setRelCenter = targetPos - center;
            float fracComplete = (Time.time - startTime) / 1;
            thisTransform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            thisTransform.position += center;
            if (fracComplete > 1)
            {
                int count = Physics.OverlapSphereNonAlloc(thisTransform.position, range, colliders,
                    thisInfo[0].lm);
                for (int i = 0; i < count; i++)
                {
                    colliders[i].TryGetComponent(out target);
                    if (target)
                        target.Hit(attackerPos, dmg);
                }

                thisInfo[0].ef?.Invoke(thisTransform.position);
                SpawnManager.Instance.projectileManagedPool.Release(this);
            }
        }
       


        private void Update()
        {
            
            duration -= Time.deltaTime;
            if(duration <0)
                SpawnManager.Instance.projectileManagedPool.Release(this);
            thisTransform = transform;

            switch (thisInfo[0].ty)
            {
                case Type.Bullet: Bullet(); break;
                case Type.Cannon: Cannon(); break;
            }
        }
    }
}