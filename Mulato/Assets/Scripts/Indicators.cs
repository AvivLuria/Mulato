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
        } else if (currObj.tag == "bounsHeart")
        {
            iTween.MoveTo(currObj.gameObject, new Vector3(4, 11, 0), 5f);
        } else
        {
            iTween.MoveTo(currObj.gameObject, new Vector3(0, 11, 0), 5f);        
        }
    }

    void Update()
    {
        if (currObj.transform.position.y > 10)
        {
            Destroy(currObj.gameObject);
        }
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
