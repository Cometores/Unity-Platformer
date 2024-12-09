using System.Collections;
using UnityEngine;

public class TrapSaw : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _sr;
    
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float cooldown = 1;
    [SerializeField] private Transform[] wayPoints;

    private Vector3[] _wayPointPositions;

    private bool _canMove = true;

    public int wayPointIndex = 1;
    public int moveDirection = 1;
    private static readonly int Active = Animator.StringToHash("active");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        UpdateWaypointsInfo();
        transform.position = _wayPointPositions[0];
    }

    private void UpdateWaypointsInfo()
    {
        _wayPointPositions = new Vector3[wayPoints.Length];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            _wayPointPositions[i] = wayPoints[i].position;
        }
    }

    private void Update()
    {
        _anim.SetBool(Active, _canMove);
        
        if (_canMove == false)
            return;
        
        transform.position = Vector2.MoveTowards(transform.position, _wayPointPositions[wayPointIndex],
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _wayPointPositions[wayPointIndex]) < .1f)
        {
            if (wayPointIndex == _wayPointPositions.Length - 1 || wayPointIndex == 0)
            {
                moveDirection *= -1;
                StartCoroutine(StopMovement(cooldown));
            }

            wayPointIndex += moveDirection;
        }
    }

    private IEnumerator StopMovement(float delay)
    {
        _canMove = false;
        
        yield return new WaitForSeconds(delay);

        _canMove = true;
        _sr.flipX = !_sr.flipX;
    }
}
