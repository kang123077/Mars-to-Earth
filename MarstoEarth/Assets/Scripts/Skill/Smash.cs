using System;
using UnityEngine;

namespace Skill
{
    public class Smash : Skill
    {
        public Smash() : base() { }
        protected override void Activate()
        {
            Debug.Log("����");
            // ����� �ִϸ��̼� ȣ��
            caster.PlaySkillClip(skillInfo.animationClip);
            caster.AddBuff(new SPC(1, 0, (caster) =>
            {
                caster.transform.position = Vector3.MoveTowards(caster.transform.position, caster.target.position, Time.deltaTime);
            }));
        }
        protected override bool GetTarget()
        {
            if (!caster.target)
                return false;



            return true;

        }

        public void GiveDemage()
        {
            // ���� ���� ���� ���鿡�� �������� �ִ� ���� ����
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
