using System;
using System.Collections;
using UnityEngine;

//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
namespace Assets.Scripts
{
    public abstract class MovingObject : MonoBehaviour
    {
        private float moveTime = 0.1f;			//Time it will take object to move, in seconds.
        public LayerMask blockingLayer;			//Layer on which collision will be checked.
        private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
        private float inverseMoveTime;			//Used to make movement more efficient.

        //Protected, virtual functions can be overridden by inheriting classes.
        protected virtual void Start ()
        {
            //Get a component reference to this object's Rigidbody2D
            rb2D = GetComponent <Rigidbody2D> ();

            //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
            inverseMoveTime = 1f / moveTime;
        }
        //Move returns true if it is able to move and false if not. 
        //Move takes parameters for row direction, column direction and a RaycastHit2D to check collision.
        //TODO : added type
        protected bool Move (int xDir, int yDir, int gridRow, int gridCol,int type)
        {
            //Store start position to move from, based on objects current transform position.
            Vector2 start = transform.position;

            // Calculate end position based on the direction parameters passed in when calling Move.
            Vector2 end = start + new Vector2 (xDir, yDir);

            //Check if anything was hit
            if(BoardManager.main.CanMoveToGridPoint(gridRow, gridCol))
            {               
                BoardManager.main.updateMovementPosition(gridRow - yDir, gridCol - xDir, gridRow, gridCol);
                //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
                StartCoroutine (SmoothMovement (end));
                
                //Return true to say that Move was successful
                return true;
            }
            //If something was hit, return false, Move was unsuccesful.
            return false;
        }
        //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
        protected IEnumerator SmoothMovement (Vector3 end)
        {
            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //While that distance is greater than a very small amount (Epsilon, almost zero):
            while(sqrRemainingDistance > float.Epsilon)
            {
                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                rb2D.MovePosition (newPostion);

                //Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;
            }
        }
        //The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
        //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
        //TODO : added type - 1 for player , 0 for enemy
        protected virtual bool AttemptMove(int xDir, int yDir, int gridRow, int gridCol,int type)
        {
            //Set canMove to true if Move was successful, false if failed.// gridRow = old gridRow + ydir , gridCol = old gridCol + xdir
            bool canMove = Move (xDir, yDir, gridRow, gridCol,type);

            return canMove;
        }
    }
}

