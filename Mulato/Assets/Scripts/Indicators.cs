using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace Assets.Scripts
{
    public class Indicators : MonoBehaviour
    {

        // Use this for initialization
        Rigidbody2D currObj;
        Canvas bar;
        public GameObject ToolTip;
        void Start()
        {
            currObj = this.GetComponent<Rigidbody2D>();
            if (currObj.tag == "Combo")
            {
                StartCoroutine(scale());
            } else if (currObj.tag == "bounsHeart")
            {
                iTween.MoveTo(currObj.gameObject, new Vector3(4, 11, 0), 5f);
            } else if (currObj.tag == "losingHeart")
            {
                iTween.MoveTo(currObj.gameObject, currObj.transform.position + new Vector3(0, 2, 0), 5f);
                StartCoroutine(delayedDestory());
            } else if (currObj.tag == "SpecialBomb")
            {
                iTween.MoveTo(currObj.gameObject, new Vector3(0, 11, 0), 5f);
            } else
            {
                StartCoroutine(arrowMove());
            }
        }

        void Update()
        {
            if (currObj.transform.position.y > 10)
            {
                Destroy(currObj.gameObject);
            }
        }
        IEnumerator delayedDestory()
        {
            yield return new WaitForSeconds(1f);
            Destroy(currObj.gameObject);
        }
        IEnumerator scale()
        {
            iTween.ScaleTo(currObj.gameObject, transform.localScale + new Vector3(0.5f, 0.5f, 0), 2.5f);
            yield return new WaitForSeconds(0.5f);
            iTween.ScaleTo(currObj.gameObject, transform.localScale - new Vector3(0.5f, 0.5f, 0), 2.5f);
            yield return new WaitForSeconds(0.5f);
            iTween.ScaleTo(currObj.gameObject, transform.localScale + new Vector3(0.5f, 0.5f, 0), 2.5f);
            Destroy(this.gameObject, 1f);
        }
        IEnumerator arrowMove()
        {
            
            for (int i = 0; i < 7; i++)
            {
                yield return new WaitForSeconds(0.4f);
                iTween.MoveBy(currObj.gameObject, new Vector3(1, 0, 0), 2f);
                yield return new WaitForSeconds(0.4f);
                iTween.MoveBy(currObj.gameObject, -new Vector3(1, 0, 0), 2f);
            }
            
            Destroy(currObj.gameObject);
        }
    }
}
