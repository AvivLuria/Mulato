using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
	public class BombScript : SceneSingleton<BombScript> {
		public int flag;

        // Use this for initialization
        void Start () {
			flag = 1;
            Destroy(this, 3f);
			flag = 0;
        }
    }
}
