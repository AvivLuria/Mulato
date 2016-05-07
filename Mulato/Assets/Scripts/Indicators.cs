using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Indicators : MonoBehaviour
{

    // Use this for initialization
    Rigidbody2D currObj;
    private bool comboBlink;

    void Start()
    {
        currObj = this.GetComponent<Rigidbody2D>();
        StartCoroutine(delayedDestory());
        if (currObj.tag == "bounsHeart")
        {
            iTween.MoveTo(currObj.gameObject, new Vector3(3.925091f, 11.45222f, 0), 5f);
        }
        else
        {
            StartCoroutine(scale());
        }
    }


    IEnumerator delayedDestory()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    IEnumerator scale()
    {

        iTween.ScaleTo(currObj.gameObject, transform.localScale + new Vector3(0.5f, 0.5f, 0), 2.5f);
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(currObj.gameObject, transform.localScale - new Vector3(0.5f, 0.5f, 0),2.5f);
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(currObj.gameObject, transform.localScale + new Vector3(0.5f, 0.5f, 0), 2.5f);
    }
}
