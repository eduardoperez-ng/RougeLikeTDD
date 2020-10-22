using UnityEngine;
using System.Collections;

namespace Completed
{
    //The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
    public abstract class MovingObject : MonoBehaviour, GameActor
    {
        public float moveTime = 0.1f;
        public LayerMask blockingLayer;
        public bool Moving => _isMoving;
        
        private BoxCollider2D _boxCollider; 
        private Rigidbody2D _rb2D;
        private float _inverseMoveTime;
        private bool _isMoving;


        protected virtual void Start()
        {
        }

        protected void Init()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rb2D = GetComponent<Rigidbody2D>();
            _inverseMoveTime = 1f / moveTime;
        }

        // Move returns true if it is able to move and false if not. 
        public bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            //Store start position to move from, based on objects current transform position.
            Vector2 start = transform.position;

            // Calculate end position based on the direction parameters passed in when calling Move.
            Vector2 end = start + new Vector2(xDir, yDir);

            // Disable the boxCollider so that linecast doesn't hit this object's own collider.
            _boxCollider.enabled = false;

            // Cast a line from start point to end point checking collision on blockingLayer.
            hit = Physics2D.Linecast(start, end, blockingLayer);

            // Re-enable boxCollider after linecast
            _boxCollider.enabled = true;

            // Check if nothing was hit and that the object isn't already moving.
            if (hit.transform == null && !_isMoving)
            {
                StartCoroutine(SmoothMovement(end));
                return true;
            }

            return false;
        }
        
        private IEnumerator SmoothMovement(Vector3 end)
        {
            _isMoving = true;

            // Calculate the remaining distance to move based on the square magnitude of the
            // difference between current position and end parameter. 
            // Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            // While that distance is greater than a very small amount (Epsilon, almost zero):
            while (sqrRemainingDistance > float.Epsilon)
            {
                // Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(_rb2D.position, end, _inverseMoveTime * Time.deltaTime);

                // Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                _rb2D.MovePosition(newPostion);

                // Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;

                // Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;
            }

            // Make sure the object is exactly at the end of its movement.
            _rb2D.MovePosition(end);

            _isMoving = false;
        }

        protected void SetPosition(int x, int y)
        {
            _rb2D.position = new Vector2(x, y);
        }

        // AttemptMove takes a generic parameter T to specify the type of
        // component we expect our unit to interact with if blocked
        // (Player for Enemies, Wall for Player).
        public virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
        {
            // Set canMove to true if Move was successful, false if failed.
            bool canMove = Move(xDir, yDir, out var hit);

            // Check if nothing was hit by linecast
            if (hit.transform == null)
                return;

            // Get a component reference to the component of type T attached to the object that was hit
            T hitComponent = hit.transform.GetComponent<T>();

            // If canMove is false and hitComponent is not equal to null,
            // meaning MovingObject is blocked and has hit something it can interact with.
            if (!canMove && hitComponent != null)
                OnCantMove(hitComponent);
        }


        // The abstract modifier indicates that the thing being modified has a missing or incomplete implementation.
        // OnCantMove will be overriden by functions in the inheriting classes.
        public abstract void OnCantMove<T>(T component) where T : Component;
        
    }
}