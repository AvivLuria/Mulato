using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Indicators : MonoBehaviour
{

    // Use this for initialization
    Rigidbody2D currObj;
    Canvas bar;

    void Start()
    {
        currObj = this.GetComponent<Rigidbody2D>();
        if (currObj.tag == "Combo")
        {
            StartCoroutine(scale());
        } else
        {
            iTween.MoveTo(currObj.gameObject, new Vector3(8, 11, 0), 5f);
            StartCoroutine(delayedDestory());
        }
    }

    IEnumerator delayedDestory()
    {
        yield return new WaitForSeconds(1.5f);
        this.transform.position = new Vector3(-100, -100, 0);
    }

    IEnumerator scale()
    {
        iTween.ScaleTo(currObj.gameObject, transform.localScale + new Vector3(0.5f, 0.5f, 0), 2.5f);
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(currObj.gameObject, transform.localScale - new Vector3(0.5f, 0.5f, 0),2.5f);
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(currObj.gameObject, transform.localScale + new Vector3(0.5f, 0.5f, 0), 2.5f);
        Destroy(this.gameObject,1f);
    }
}
