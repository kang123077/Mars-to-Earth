using System;
using System.Collections;
using Skill;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        public ItemType type;
        
        private SPC[] spcs;

        static ItemInfo[] infos = new ItemInfo[3];
        float[] temps= new float[2];

        private void Awake()        
        {
            for(int i = 0; i< ResourceManager.Instance.itemInfos.Length;i++)
            {
                infos[i] = ResourceManager.Instance.itemInfos[i];
            }

            spcs = new SPC[3]
            {
                 new((ch) => spcs[0].Tick((stack)=>{ch.hp+=stack* ch.characterStat.maxHP*0.01f; }),infos[0].SPC_Sprite),
                 new( (ch) => {
                     temps[0]= ch.speed;
                     ch.speed+= temps[0]*0.2f;
                 }, (ch) =>ch.speed-=temps[0]*0.2f, infos[1].SPC_Sprite),
                 new((ch) => {
                     temps[1]= ch.dmg;
                     ch.dmg += temps[1] * 0.2f;
                 },(ch)=>ch.dmg-=temps[1]*0.2f , infos[2].SPC_Sprite),
            };
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Use(Character.Player player)
        {
            ReleaseEffect effect = SpawnManager.Instance.GetEffect(player.transform.position, infos[(int)type].targetParticle,(int)CombatEffectClip.itemUse,1,20);
            effect.transform.SetParent(player.transform,true);
            spcs[(int)type].Init(20);
            switch (type)
            {
                case ItemType.Heal:
                    player.MaxHp += 5;
                    break;
                case ItemType.Boost:
                    player.speed += 0.1f;
                    break;
                case ItemType.PowerUp:
                    player.dmg += 1;
                    break;
            }
            player.AddBuff(spcs[(int)type]);
            SpawnManager.Instance.itemPool.Add(this);
            MapInfo.core++;
            gameObject.SetActive(false);
        }        
    }
}