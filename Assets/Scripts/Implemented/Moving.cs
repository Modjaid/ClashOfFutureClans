using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour, IMoveable
{
    private IEnumerator moveRoutine;
    [SerializeField]
    private float speed = 0.1f;
    private List<Vector3> path;
    private Animator animator;
    private Animator Animator
    {
        get
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            return animator;
        }
    }

    private IEnumerator MoveRoutine()
    {
        int i = 0;
        Animator.SetFloat("Speed", speed);
        while (path != null && i != path.Count - 1)
        {
            if(transform.position != path[i])
                transform.forward = path[i] - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, path[i], Time.deltaTime * speed);
            if (path[i] == transform.position)
                i++;
            yield return null;
        }
        Animator.SetFloat("Speed", -1);
        transform.hasChanged = false;
        moveRoutine = null;
    }

    public void Move(List<Vector3> path)
    {
        this.path = path;
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            Animator.SetFloat("Speed", -1);
        }
        if (transform.position != path[path.Count - 1])
        {
            moveRoutine = MoveRoutine();
            StartCoroutine(moveRoutine);
        }
    }
}
