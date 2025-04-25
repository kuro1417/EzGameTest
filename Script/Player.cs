using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    public static Player Instance { get; private set; }
    private bool isWalking;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotate();
    }

    private void HandleMovement()
    {
        if (!BoxingGameManager.Instance.IsGamePlaying()) return;

        Vector2 inputVector = GameInput.Instance.getMovermentNormalize();
        Vector3 moveDirection = new Vector3(inputVector.x,0f,inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;

        isWalking = moveDirection != Vector3.zero;

        float playerRadius = 0.8f;

        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirection, Quaternion.identity, moveDistance, collisionsLayerMask);

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
    }

    private void HandleRotate()
    {
        if (!BoxingGameManager.Instance.IsGamePlaying() || BoxingGameManager.Instance.IsPause()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            transform.LookAt(new Vector3(point.x,transform.position.y,point.z));
        }
    }
    public bool IsWalking()
    {
        return isWalking;
    }
}
