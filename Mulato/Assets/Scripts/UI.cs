using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Ui : MonoBehaviour
{
    private static bool click = true;
    private Vector2 m_inputMouse;
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) && click)
        {
            //get the hit position
             m_inputMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(m_inputMouse, Vector2.zero);
            if (hit.collider != null && hit.collider.tag.Equals("floor") && GameManager.main.m_CurrentBoard.GetComponent<BoardManager>().GetGridPointObject
                (hit.collider.gameObject.GetComponent<floor>().gridRow, 
                    hit.collider.gameObject.GetComponent<floor>().gridCol) == 0) //we have a hit!!!
            {                
                BombManager.main.DeployBomb(hit.transform.position.x ,hit.transform.position.y,
                    hit.collider.gameObject.GetComponent<floor>().gridRow, 
                    hit.collider.gameObject.GetComponent<floor>().gridCol);            
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.timeScale == 1.0F)
            {
                Time.timeScale = 0.5F;
            }
            else
            {
                Time.timeScale = 1.0F;
                Time.fixedDeltaTime = 0.02F*Time.timeScale;
            }
        }

    }

    public static void activeUI(bool input)
    {
        click = input;
    }
}
