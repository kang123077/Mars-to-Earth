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
            Debug.Log("��ų �ߵ� : " + skillInfo.name);
            // ����� �ִϸ��̼� ȣ��
            caster.PlaySkillClip(2, "Jump");

            // ���� ���� ���� ���鿡�� �������� �ִ� ���� ����
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
