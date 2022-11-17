using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

namespace Test.LavaProject.Farm.Mechanica_Spawner.Cells.Plants
{
    public class PlantTile : MonoBehaviour
    {
        private PlantType _currentPlantType;

        private IUIPresentor _uIPresentor;

        [SerializeField] private Transform[] _prefabPlant;
        [SerializeField] private Vector3 _maxSizePrefab;

        private float _time;
        private float _timeUpdateUI = 0.1f;
        private int _experience;
        private int _carrotCount;
        private int _selectedCell;

        public void SetDataPlant(PlantType plant, float time, int experience, IUIPresentor presentor, int selectedCell)
        {
            _currentPlantType = plant;
            _time = time;
            _experience = experience;
            _uIPresentor = presentor;
            _selectedCell = selectedCell;
            _uIPresentor.RegisterPlant(this);

            SetCarrotCount();
            PlantProcess(time);
            StartCoroutine(Timer());
        }
        private void SetCarrotCount()
        {
            if (_currentPlantType != PlantType.Carrot)
                return;

            _carrotCount = _prefabPlant.Length;
        }

        private void PlantProcess(float time)
        {
            for (int i = 0; i < _prefabPlant.Length; i++)
            {
                _prefabPlant[i].DOScale(_maxSizePrefab, time);
            }
        }

        private IEnumerator Timer()
        {
            while (_time > 0)
            {
                _time -= _timeUpdateUI;
                yield return new WaitForSeconds(_timeUpdateUI);
            }

            yield return new WaitForSeconds(_time);

            _uIPresentor.FinishedFlowerBeds(_selectedCell, _experience, this);
        }

        public void GetCarrotResult()
        {
            _uIPresentor.UnRegisterPlant(this, _currentPlantType, _carrotCount);
        }

        public void DestroyThisPlant()
        {
            DestroyPlant();
        }

        public PlantType GetPlantType()
        {
            return _currentPlantType;
        }

        public Transform GetCurrentPosition()
        {
            return transform;
        }

        public Transform GetUITimerPosition()
        {
            return transform;
        }

        public float GetCurrentTime()
        {
            if (_time <= 0)
                return 0;
            else
            {
                var correctTime = Math.Round(_time, 1);
                return (float)correctTime;
            }
        }

        private void DestroyPlant()
        {
            Destroy(gameObject);
        }
    }
}