using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class UI : MonoBehaviour
{
    private Vector2 m_inputMouse;
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //get the hit position
            m_inputMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(m_inputMouse, Vector2.zero);
            if (hit.collider != null && hit.collider.tag.Equals("floor")) //we have a hit!!!
            {                
                BombManager.main.DeployBomb((int)hit.transform.position.x,(int)hit.transform.position.y);            
            }
        }
    }
}
