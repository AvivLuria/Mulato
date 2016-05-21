using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ToolTip : MonoBehaviour
    {

        public Text toolTipText;
        // Use this for initialization
        void Start()
        {
            
        }

        public void changeToolTipText(string text)
        {
            toolTipText = GetComponent<Text>();
            toolTipText.text = text;
        }
    }
}
