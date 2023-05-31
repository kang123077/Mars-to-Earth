using UnityEngine;

namespace Projectile
{
    public class Hyperion : Installation
    {
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
        public ParticleSystem particleRange;

        public override void Init(int lm, float dg, float rg, float dr, float sp, bool enforce)
        {
            base.Init(lm, dg, rg, dr, sp, enforce);
            if (!particleRange)
            {
                particleRange = Instantiate(ResourceManager.Instance.skillInfos[(int)SkillName.Hyperion].effects[1],
                    thisTransform.position - new Vector3(0, thisTransform.position.y, 0), Quaternion.identity);
                particleRange.transform.SetParent(thisTransform);
            }

            projectileInfo = new ProjectileInfo(layerMask,
                ResourceManager.Instance.projectileMesh[(int)projectileMesh.Bullet1].sharedMesh,
                Type.Bullet, (point) =>
                {
                    int count = Physics.OverlapSphereNonAlloc(point, range, colliders, layerMask);
                    for (int i = 0; i < count; i++)
                    {
                        colliders[i].TryGetComponent(out target);
                        if (target)
                            target.Hit(point, dmg, 0);
                    }

                    SpawnManager.Instance.GetEffect(point,
                        ResourceManager.Instance.skillInfos[(int)SkillName.Hyperion].effects[0], (int)CombatEffectClip.explosion2, range * 0.4f);
                });
            atkSpd = 10 * (1 / speed);
            curPorts = new Transform[16];
            for (int i = 0; i < 16; i++)
            {
                GameObject port = new();
                port.transform.position = thisTransform.position + range * 2 * ports[i];
                curPorts[i] = port.transform;
                port.layer = 8;
                port.transform.SetParent(thisTransform);
            }
        }

        private void Update()
        {
            BaseUpdate();
            thisTransform.position += enforce ? Vector3.MoveTowards(thisTransform.position,
                    SpawnManager.Instance.player.target.position, Time.deltaTime * speed * 0.2f) :
            thisTransform.forward * (Time.deltaTime * speed * 0.2f);

            eleapse += Time.deltaTime;
            if (eleapse > atkSpd)
            {
                eleapse -= atkSpd;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 shotPoint = curPorts[i * 4 + UnityEngine.Random.Range(1, 5) - 1].position;
                    SpawnManager.Instance.Launch(shotPoint, Vector3.down, dmg, 2, 30 + speed, range * 0.1f,
                        ref projectileInfo);
                }
            }
        }
    }
}