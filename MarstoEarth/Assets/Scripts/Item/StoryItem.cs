using Skill;
using UnityEngine;

namespace Item
{

    public class StoryItem : MonoBehaviour, IItem
    {
        [SerializeField]
        private ItemInfo storyItemInfo;

        public void Use(Character.Player player)
        {
            ReleaseEffect effect =
                SpawnManager.Instance.GetEffect(player.transform.position,
                storyItemInfo.targetParticle, (int)CombatEffectClip.itemUse, 1, 1);
            effect.transform.SetParent(player.transform, true);
            PlayerPrefs.SetInt("storyValue", ++MapInfo.storyValue);
            DamageText dt = UIManager.Instance.combatUI.DMGTextPool.Get();
            dt.transform.position = player.transform.position + Vector3.up;
            dt.gameObject.SetActive(true);
            dt.lifeTime = 10f;
            dt.moveSpeed = 1f;
            dt.alphaSpeed = 0.2f;
            dt.text.text = "기록 데이터 획득";
            gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.eulerAngles += Vector3.up * (Time.deltaTime * 10);
        }
    }
}