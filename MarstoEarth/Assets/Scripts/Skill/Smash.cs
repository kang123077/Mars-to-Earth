using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {

        public Smash(SkillInfo skillInfo) : base ()
        {
            this.skillInfo = skillInfo;
        }
        protected override void Activate()
        {
            Debug.Log("Target position: " + caster.target.position);
            Debug.Log("스킬 발동 : " + skillInfo.name);
            // 재생할 애니메이션 호출
            caster.PlaySkillClip(2, "Jump");

            // 일정 범위 내의 적들에게 데미지를 주는 로직 구현
        }
        protected override bool GetTarget()
        {
            if (!caster.target)
                return false;
            return true;

        }
        public void GiveDemage()
        {
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
}
