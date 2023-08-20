using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    public NavMeshAgent agent;

    public bool velocity;
    public bool desiredVelocity;
    public bool path;

    private void OnDrawGizmos()
    {
        if (velocity) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        if (desiredVelocity) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        if (path) {
            Gizmos.color = Color.blue;
            var agentPath = agent.path;
            Vector3 prevCorner = transform.position;
            foreach(var corner in agentPath.corners) {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
