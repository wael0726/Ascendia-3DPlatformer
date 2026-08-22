using UnityEngine;
using Utilities;
namespace Platformer {
    public class ConeDetectionStrategy : IDetectionStrategy {
        readonly float detectionAngle;
        readonly float detectionRadius;
        readonly float innerDetectionRadius;
        readonly LayerMask obstacleMask;
        
        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius, LayerMask obstacleMask) {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
            this.innerDetectionRadius = innerDetectionRadius;
            this.obstacleMask = obstacleMask;
        }
        
        public bool Execute(Transform player, Transform detector, CountdownTimer timer) {
            if (timer.IsRunning) return false;
            
            var directionToPlayer = player.position - detector.position;
            var angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);
            
            // If the player is not within the detection angle + outer radius (aka the cone in front of the enemy),
            // or is within the inner radius, return false
            if ((!(angleToPlayer < detectionAngle / 2f) || !(directionToPlayer.magnitude < detectionRadius))
                && !(directionToPlayer.magnitude < innerDetectionRadius)) 
                return false;
            
            if (!HasLineOfSight(detector.position, player.position))
                return false;
            
            timer.Start();
            return true;
        }
        
        bool HasLineOfSight(Vector3 detectorPos, Vector3 playerPos) {
            Vector3 direction = playerPos - detectorPos;
            float distance = direction.magnitude;
            
            if (Physics.Raycast(detectorPos, direction.normalized, out RaycastHit hit, distance, obstacleMask)) {
                return false;
            }
            return true;
        }
    }
}
