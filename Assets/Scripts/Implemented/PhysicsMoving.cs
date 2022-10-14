using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;

public class PhysicsMoving : MonoBehaviour, IMoveable
{
    private IEnumerator moveRoutine;
    [SerializeField]
    private float speed = 0.1f;
    private List<Vector3> path;
    private Animator animator;
    private int sid;
    private int index;
    private Animator Animator
    {
        get
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            return animator;
        }
    }


    private void Start()
    {
        sid = Simulator.Instance.addAgent(new RVO.Vector2(transform.position.x, transform.position.z), 5, 10, 5.0f, 5.0f, GetComponent<CapsuleCollider>().radius, speed / 10f, new RVO.Vector2(0.0f, 0.0f));
    }

    private void Update()
    {
        if(path != null && index <= path.Count - 1)
        {
            Move(path[index]);
        }
        else
        {
            path = null;
            index = 0;
            Move(transform.position);
        }
    }

    private void Move(Vector3 to)
    {
        RVO.Vector2 pos = Simulator.Instance.getAgentPosition(sid);
        var delta = pos - new RVO.Vector2(transform.position.x, transform.position.z);
        var curSpeed = new UnityEngine.Vector2(delta.x, delta.y).magnitude;
        Animator.SetFloat("Speed", curSpeed > Time.deltaTime * speed ? curSpeed: -1);
        transform.position = new Vector3(pos.x, transform.position.y, pos.y);
        if (curSpeed > Time.deltaTime * speed)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(delta.x, 0, delta.y).normalized, Mathf.PI / 50, 0);
        }
        if (to == transform.position)
        {
            Simulator.Instance.setAgentPrefVelocity(sid, new RVO.Vector2(0, 0));
            return;
        }

        RVO.Vector2 goalVector = new RVO.Vector2(to.x, to.z) - Simulator.Instance.getAgentPosition(sid);
        if (RVOMath.absSq(goalVector) > 1.0f)
        {
            goalVector = RVOMath.normalize(goalVector);
        }

        Simulator.Instance.setAgentPrefVelocity(sid, goalVector);


        /* Perturb a little to avoid deadlocks due to perfect symmetry. */
        float angle = Random.value * 2.0f * Mathf.PI;
        float dist = Random.value * 0.0001f;

        Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) +
                                                     dist *
                                                     new RVO.Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

        if (Vector3.Distance(to, transform.position) < 0.5f)
            index++;
    }

    private void OnDisable()
    {
        Simulator.Instance.delAgent(sid);
    }

    public void Move(List<Vector3> path)
    {
        this.path = path;
        SimplifyPath();
        index = 0;
    }


    public void SimplifyPath()
    {
        if (path == null)
            return;
        for (int i = path.Count - 1; i > 1; i--)
        {
            if(CanBeSimplified(path[i], path[i - 1], path[i - 2]))
            {
                path.RemoveAt(i - 1);
            }
        }
    }

    private bool CanBeSimplified(Vector3 vec1, Vector3 vec2, Vector3 vec3)
    {
        if(((vec3.x - vec1.x) / (vec2.x - vec1.x) - (vec3.y - vec1.y) / (vec2.y - vec1.y)) < 0.01f)
        {
            return true;
        }
        return false;
    }
}
