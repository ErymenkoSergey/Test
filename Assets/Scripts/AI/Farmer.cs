using Test.LavaProject.Farm.Mechanica_Spawner;
using UnityEngine;
using UnityEngine.AI;

namespace Test.LavaProject.Farm.Mechanica.AI
{
    public class Farmer : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;

        private const float _DISTANCE_TO_CELL = 2f;
        private Spawner _spawner;

        private bool _isMove;
        private Transform _newPoint;
        private Harvest _harvest;

        public void SetInfo(Spawner spawner)
        {
            _spawner = spawner;
            _spawner.RegisterFarmer(this);
        }

        private void Update()
        {
            if (!_isMove)
                return;

            FarmerMoved();
        }

        public void StartMoved(bool isMove, Transform cellPosition, Harvest harvest)
        {
            _isMove = isMove;
            _newPoint = cellPosition;
            _harvest = harvest;
        }

        private void FarmerMoved()
        {
            if (Vector3.Distance(transform.position, _newPoint.position) < _DISTANCE_TO_CELL)
            {
                if (_harvest == Harvest.PlantCrop)
                {
                    _animator.SetFloat("Walk", 0);
                    _spawner.StartDisembarkation();
                }

                if (_harvest == Harvest.PickUpHarvest)
                    _spawner.StartHarvesting();

                _isMove = false;
            }
            else
            {
                _agent.SetDestination(_newPoint.position);
                _animator.SetFloat("Walk", 1);
            }
        }

        public Transform GetFarmerPosition()
        {
            return transform;
        }
    }
}