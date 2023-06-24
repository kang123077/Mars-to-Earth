using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{

    public class StoryItem : MonoBehaviour, IItem
    {
        public ItemInfo storyItemInfo;
        public void Use(Character.Player player)
        {
            ReleaseEffect effect = SpawnManager.Instance.GetEffect(player.transform.position, storyItemInfo.targetParticle, (int)CombatEffectClip.itemUse, 1, 1);
            effect.transform.SetParent(player.transform, true);
            MapInfo.storyValue++;
            PlayerPrefs.SetInt("storyValue", MapInfo.storyValue);
            gameObject.SetActive(false);
        }
        private void Update()
        {
            transform.eulerAngles += Vector3.up * (Time.deltaTime * 10);
        }
    }
}