using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEditor;
using UnityEditor.Animations;
using DG.Tweening;

public class PlayerInput : MonoBehaviour
{
    public int playerId = 0;
    public Player player; // The Rewired Player

    public Vector3 inputDirection;

    public Vector3 currentHeading;

    public Vector3 previousHeading;

    public float playerSpeed;

    public float rotationSpeed;

    Rigidbody myRb;

    public int maxReflectionCount = 5;
    public float maxStepDistance = 100;

    public float myVelocity;

    public LayerMask collisionMask;

    public float slowFactor = 10;

    public LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();

    bool canMove = false;

    public bool touchStarted = false;

    public float modelRotationSpeed = 0.7f;

    public Transform graphicObject;

    public Animator animator;

    public bool tiltBodyToSurface = true;

    public bool doJump;
    public bool doAnimate;

    public Ease easeType = Ease.InOutSine;

    public bool isplaying;

    public float reactDistance = 3f;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
        myRb = GetComponent<Rigidbody>();
       // lineRenderer = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        

    }

    public void DrawPath(Vector3 position, Vector3 direction, int reflectionsRemaining, int index)
    {
        position.y = 0.5f;
        lineRenderer.SetPosition(0, position);
        //lineRenderer.SetPosition(1, position + currentHeading * 5);

        Vector3 direction2 = Vector3.zero;

        // DrawPath(position, direction, reflectionsRemaining - 1, index + 1);

        //Ray ray2 = new Ray(transform.position, previousHeading);
        //RaycastHit hitInfo2;


        //if (Physics.SphereCast(position, 0.2f, direction, out hit, 10)) //(Physics.Raycast(ray2, out hitInfo2, 5f, collisionMask))
        //{

        //}

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.SphereCast(position, 0.35f, direction, out hit, 10)) //(Physics.Raycast(ray, out hit, 10f))
        {
            direction2 = Vector3.Reflect(direction, hit.normal);
            var hitPoint = hit.point;
            hitPoint.y = 0.5f;
            lineRenderer.SetPosition(1, hitPoint);
            var distance = 10f - (hitPoint - position).magnitude;
            lineRenderer.SetPosition(2, hitPoint + direction2 * (distance + 2.5f));

            
            //var newRotation = Quaternion.LookRotation(currentHeading).eulerAngles;
            //newRotation.x = 0;
            //newRotation.z = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.deltaTime);
        }
        else
        {
            lineRenderer.SetPosition(1, position + direction * 3);
            lineRenderer.SetPosition(2, position + direction * 10);

            //var newRotation = Quaternion.LookRotation(currentHeading).eulerAngles;
            //newRotation.x = 0;
            //newRotation.z = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.deltaTime);
        }

        
    }

    public void SetTouchStarted(bool value)
    {
        touchStarted = value;
    }
    public void HandleInput()
    {
        inputDirection = new Vector3(player.GetAxisRaw("MoveHorizontal"), 0, player.GetAxisRaw("MoveVertical"));

        float horizontalAxis = player.GetAxisRaw("MoveHorizontal");
        float verticalAxis = player.GetAxisRaw("MoveVertical");
        //Move if the input is not zero
        if (touchStarted)
        {

            if (Time.timeScale == 1.0f)
            {

                float newTimeScale = Time.timeScale / slowFactor;
                //assign the 'newTimeScale' to the current 'timeScale'  
                Time.timeScale = newTimeScale;
                //proportionally reduce the 'fixedDeltaTime', so that the Rigidbody simulation can react correctly  
                Time.fixedDeltaTime = Time.fixedDeltaTime / slowFactor;
                //The maximum amount of time of a single frame  
                Time.maximumDeltaTime = Time.maximumDeltaTime / slowFactor;


            }

            if (inputDirection != Vector3.zero)
            {
                canMove = false;
                //inputDirection = Camera.main.transform.TransformDirection(inputDirection);
                //inputDirection.Normalize();

                //inputDirection.y = 0.0f;

                //currentHeading = inputDirection;


                //assuming we only using the single camera:

                lineRenderer.positionCount = 3;

                var camera = Camera.main;

                //camera forward and right vectors:
                var forward = camera.transform.forward;
                var right = camera.transform.right;

                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();


                var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

                currentHeading = desiredMoveDirection.normalized;

                // DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
                DrawPath(this.transform.position, currentHeading, maxReflectionCount, 0);
                //touchStarted = false;
            }


            


            
        }
        else
        {
            previousHeading = currentHeading;
            canMove = true;
            points.Clear();
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
            if ((Time.timeScale != 1.0f) && (Time.fixedDeltaTime != 0.01f)) //the game is running in slow motion  
            {
                //reset the values  
                Time.timeScale = 1.0f;
                //Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactorValue;
                Time.fixedDeltaTime = 0.01f;
                //Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactorValue;
                Time.maximumDeltaTime = 0.3333333f;

            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);

        DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);


    }

    public void HandleMovement()
    {
        if(canMove)
        {
            myRb.velocity = currentHeading * playerSpeed;
            //transform.rotation = Quaternion.LookRotation(currentHeading);


            var newRotation = Quaternion.LookRotation(currentHeading).eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.deltaTime);
        }
        else
        {
            

                myRb.velocity = previousHeading * playerSpeed;
            //transform.rotation = Quaternion.LookRotation(currentHeading);

            var newRotation = Quaternion.LookRotation(previousHeading).eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.deltaTime);

            //var newRotation = Quaternion.LookRotation(previousHeading).eulerAngles;
            //newRotation.x = 0;
            //newRotation.z = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), rotationSpeed * Time.deltaTime);
        }
        


        myVelocity = myRb.velocity.magnitude;
    }

    public void HandleCollision()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position, 0.2f, transform.forward, out hitInfo, 0.5f, collisionMask)) //(Physics.Raycast(ray, out hitInfo, 0.5f, collisionMask))
        {
           // Debug.Log("Hit Body : " + hitInfo.normal);
            OnHitObject(hitInfo);


        }

        Ray ray2 = new Ray(transform.position, previousHeading);
        RaycastHit hitInfo2;


        if (Physics.SphereCast(transform.position, 1f, previousHeading, out hitInfo2, reactDistance, collisionMask)) //(Physics.Raycast(ray2, out hitInfo2, 5f, collisionMask))
        {
            if(tiltBodyToSurface)
            {
                Vector3 targetDirection = hitInfo2.normal;
                Vector3 bodyUp = transform.up;


                var newRotation = Quaternion.FromToRotation(bodyUp, targetDirection) * transform.rotation;

                graphicObject.rotation = Quaternion.Slerp(graphicObject.rotation, newRotation, modelRotationSpeed * Time.deltaTime);
            }
               

            if(doAnimate)
            {
                //animator.SetTrigger("Jump");

                animator.SetBool("jump", true);
            }

            


            if(doJump)
            {
                graphicObject.DOLocalMoveY(1.5f, 0.3f).SetEase(easeType);
            }

           // playerSpeed = 15;

        }
        else
        {

            if (tiltBodyToSurface)
            {
                Vector3 targetDirection = Vector3.up;
                Vector3 bodyUp = transform.up;


                var newRotation = Quaternion.FromToRotation(bodyUp, targetDirection) * transform.rotation;

                graphicObject.rotation = Quaternion.Slerp(graphicObject.rotation, newRotation, modelRotationSpeed * Time.deltaTime);
            }
            graphicObject.DOLocalMoveY(0f, 0.2f);
            //graphicObject.DOLocalMoveY(2f, 0.2f).OnComplete(()=>ReturnToGround());

            if (doAnimate)
            {
                animator.SetBool("jump", false);
            }
                

           // playerSpeed = 10;
        }
    }

    public void ReturnToGround()
    {
        graphicObject.DOLocalMoveY(0f, 0.2f).SetEase(Ease.InSine);
    }

        private void OnHitObject(RaycastHit hitInfo)
    {
        //Debug.Log("Hit Body Normal: " + hitInfo.normal);
        currentHeading = Vector3.Reflect(currentHeading, hitInfo.normal).normalized;
        previousHeading = Vector3.Reflect(previousHeading, hitInfo.normal).normalized;

        // if(doAnimate)
        // {
        //     animator.SetTrigger("Attack");
        // }

        //graphicObject.DOLocalMoveY(2f, 0.2f);
        //animator.SetBool("jump", false);

        // if(!isplaying)
        // {

        //     StartCoroutine(Attack(hitInfo.normal,tempcurrentHeading,temppreviousHeading));

        // }


        


    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if(reflectionsRemaining == 0)
        {
            return;
        }

        Vector3 startingPosition = position;
        //Raycast to detect reflection

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, maxStepDistance))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPosition, position );

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }


    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            currentHeading = Vector3.Reflect(currentHeading, collision.GetContact(0).normal).normalized;
            previousHeading = Vector3.Reflect(previousHeading, collision.GetContact(0).normal).normalized;


            if(doAnimate)
            {
                // animator.SetTrigger("Attack");

                //animator.SetBool("jump", false);

                // StartCoroutine(Attack(collision.GetContact(0).normal));
            }
           
            //graphicObject.DOLocalMoveY(2f, 0.2f).SetEase(Ease.InSine).OnComplete(() => ReturnToGround());
        }

    }

    public IEnumerator Attack(Vector3 normal, Vector3 _currentHeading, Vector3 _previousHeading)
    {

        isplaying = true;
        if (doAnimate)
        {
            animator.SetTrigger("Attack");
        }

        //yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        

        yield return new WaitForSeconds(0.1f);
        Reflect(normal,_currentHeading,_previousHeading);

        isplaying = false;


    }

    public void Reflect(Vector3 normal, Vector3 _currentHeading, Vector3 _previousHeading)
    {
        currentHeading = _currentHeading;//Vector3.Reflect(currentHeading, normal).normalized;
        previousHeading = _previousHeading;//Vector3.Reflect(previousHeading, normal).normalized;
    }

}
