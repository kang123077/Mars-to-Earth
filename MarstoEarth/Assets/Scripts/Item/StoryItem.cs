using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{

    public class StoryItem : MonoBehaviour, IItem
    {
        public void Use(Character.Player player)
        {

        }
        private void Update()
        {
            transform.eulerAngles += Vector3.up * (Time.deltaTime * 10);
        }
    }

}