using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {
        protected override void Activate()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(caster.target.position), out RaycastHit hit))
            {
                Debug.Log("Target position: " + caster.target.position);
                Debug.Log("스킬 발동 : " + skillInfo.name);

                // 재생할 애니메이션 호출

                // 일정 범위 내의 적들에게 데미지를 주는 로직 구현
                Collider[] colliders = Physics.OverlapSphere(caster.target.position, skillInfo.range, LayerMask.GetMask("Monster"));
                foreach (Collider collider in colliders)
                {
                    Character.Monster enemy = collider.GetComponent<Character.Monster>();
                    if (enemy != null)
                    {
                        enemy.Hit(caster.transform, skillInfo.dmg, 0);
                    }
                }
            }
        }
        protected override bool GetTarget()
        {
            if (!caster.target)
                return false;



            return true;

        }
    }
}
