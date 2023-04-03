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
                Debug.Log("��ų �ߵ� : " + skillInfo.name);

                // ����� �ִϸ��̼� ȣ��

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
        protected override bool GetTarget()
        {
            if (!caster.target)
                return false;



            return true;

        }
    }
}
