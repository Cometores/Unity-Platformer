using Game._Scripts.Enemy.Movement;

namespace Game._Scripts.Enemy
{
    public class MushroomEnemy : Enemy
    {
        private IMovement _groundMovement;

        protected override void Awake()
        {
            base.Awake();
            _groundMovement = new GroundMovement(this);
        }

        protected override void Update()
        {
            base.Update();
            
            if (!IsDead && IdleTimer <= 0) 
                _groundMovement.ManageEnemyMovement();
        }
    }
}