using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
using Vector2 = RVO.Vector2;

public class ObstacleAgent : MonoBehaviour
{
    private void OnEnable()
    {
        var collider = GetComponentInChildren<BoxCollider>(true);
        var size = collider.size;
        size.Scale(collider.transform.lossyScale * 0.5f);
        size += new Vector3(0.5f, 0.5f, 0.5f);
        var index = Simulator.Instance.addAgent(new Vector2(transform.position.x, transform.position.z), 7, 6, 5.0f, 100, new UnityEngine.Vector2(size.x, size.z).magnitude, 0, new Vector2(0, 0));

        GetComponent<Health>().OnDead += () => Simulator.Instance.delAgent(index);
    }
}
