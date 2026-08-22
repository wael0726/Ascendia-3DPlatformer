using UnityEngine;
using UnityEngine.AI;
namespace Platformer {
    public class EnemyChaseState : EnemyBaseState {
        readonly NavMeshAgent agent;
        readonly Transform player;
        
        float bezierT = 0f;
        const float curveSpeed = 1.5f;
        const float curveOffset = 2f;
        
        public EnemyChaseState(Enemy enemy, NavMeshAgent agent, Transform player) : base(enemy) {
            this.agent = agent;
            this.player = player;
        }
        
        public override void OnEnter() {
            Debug.Log("Chase");
            bezierT = 0f;
        }
        
        public override void Update() {
            Vector3 start = agent.transform.position;
            Vector3 end = player.position;
            Vector3 midpoint = (start + end) / 2f;
            
            // Offset the control point perpendicular to the chase direction to create a curve
            Vector3 perpendicular = Vector3.Cross(end - start, Vector3.up).normalized;
            Vector3 controlPoint = midpoint + perpendicular * curveOffset;
            
            bezierT = Mathf.Clamp01(bezierT + Time.deltaTime * curveSpeed);
            Vector3 curvedTarget = CalculateBezierPoint(start, controlPoint, end, bezierT);
            
            agent.SetDestination(curvedTarget);
        }
        
        Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
            float u = 1f - t;
            return (u * u * p0) + (2f * u * t * p1) + (t * t * p2);
        }
    }
}
