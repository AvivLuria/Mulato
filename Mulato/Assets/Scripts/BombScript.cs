using UnityEngine;

namespace Assets.Scripts
{
    public class BombScript : MonoBehaviour {

        // Use this for initialization
        void Start () {
            Destroy(this, 3f);
        }
    }
}
