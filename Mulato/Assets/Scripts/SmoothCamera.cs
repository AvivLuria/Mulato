using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class SmoothCamera : MonoBehaviour
{

    private Vector2 velocity;
    public GameObject player;
    public float x;
    public float y;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, x);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, y);
        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}