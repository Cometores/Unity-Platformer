using UnityEngine;

namespace Game._Scripts.Enemy.Movement
{
    public class GroundMovement : IMovement
    {
        private Enemy _enemy;
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public GroundMovement(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void ManageEnemyMovement()
        {
            WalkTowards();

            if (_enemy.IsGrounded)
                TurnAroundIfNeeded();
            
            SynchronizeAnimation();
        }

        private void TurnAroundIfNeeded()
        {
            if (!_enemy.IsGroundInFront || _enemy.IsWallDetected)
            {
                _enemy.Flip();
                _enemy.IdleTimer = _enemy.idleDuration;
                _enemy.Rb.linearVelocity = Vector2.zero;
            }
        }

        private void WalkTowards()
        {
            if (_enemy.IsGroundInFront)
                _enemy.Rb.linearVelocityX = _enemy.moveSpeed * _enemy.FacingDir;
        }
        
        private void SynchronizeAnimation()
        {
            _enemy.Anim.SetFloat(XVelocity, _enemy.Rb.linearVelocityX);
        }
    }
}