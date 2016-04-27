using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utils;
namespace Assets.Scripts
{
    public class TextLife : SceneSingleton<GameManager>
    {

        public Text lifeText;
        public GameObject gameManager;
        // Use this for initialization
        void Start()
        {
            lifeText = GetComponent<Text>();
        }

        void Update()
        {
            lifeText.text = GameManager.main.life.ToString();
        }
    }
}