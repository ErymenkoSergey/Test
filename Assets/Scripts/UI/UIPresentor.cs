using Test.LavaProject.Farm.Mechanica.Score;
using Test.LavaProject.Farm.Mechanica_Spawner.Cells.Plants;
using Test.LavaProject.Farm.UI;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Test.LavaProject.Farm.Mechanica_UI
{
    public class UIPresentor : MonoBehaviour, IUIPresentor
    {
        private ScoreSystem _scoreSystem;

        [SerializeField] private GameObject _spawner;
        private ISpawn _iSpawn;

        [SerializeField] private GameObject _cameraControl;
        private ICameraControl _iCameraControl;

        private Camera _camera;

        [SerializeField] private GameObject _timerPrefab;
        [SerializeField] private RectTransform _fieldVisualize;

        private Dictionary<PlantTile, UITimer> _plants = new Dictionary<PlantTile, UITimer>();
        private Dictionary<int, PlantTile> _plantsReady = new Dictionary<int, PlantTile>();

        private float _fps = 60f;

        private void Start()
        {
            SetLinks();
        }

        private void SetLinks()
        {
            if (_spawner.TryGetComponent(out ISpawn spawn))
                _iSpawn = spawn;

            if (_cameraControl.TryGetComponent(out ICameraControl control))
                _iCameraControl = control;

            _camera = _iCameraControl.GetCamera();
        }

        private void Update()
        {
            UpdateUIPositionIcon();
        }

        public void RegisterPlant(PlantTile plant)
        {
            UITimer newTimer = Instantiate(_timerPrefab, _fieldVisualize).GetComponent<UITimer>();

            newTimer.transform.localScale = Vector3.zero;
            newTimer.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);

            _plants.Add(plant, newTimer);
        }

        public void FinishedFlowerBeds(int index, int experience, PlantTile tile)
        {
            _plantsReady.Add(index, tile);
            _iSpawn.SetStatusReady(index);
            SetScoreExperience(experience);
            DelUITimer(ref tile);
        }

        private void DelUITimer(ref PlantTile tile)
        {
            var timer = _plants[tile];
            _plants.Remove(tile);

            Destroy(timer.gameObject);
        }

        public void UnRegisterPlant(PlantTile inputPlant, PlantType type, int carrots)
        {
            if (type == PlantType.Carrot)
                SetCarrotCount(carrots);
        }

        public void ReadinessCheck(int index)
        {
            _plantsReady.TryGetValue(index, out var result);

            if (result.GetPlantType() == PlantType.Carrot)
            {
                _plantsReady[index].GetCarrotResult();
                RemoveKey(index);
            }
            if (result.GetPlantType() == PlantType.Grass)
            {
                RemoveKey(index);
            }
            if (result.GetPlantType() == PlantType.Tree)
            {
                Debug.Log("We do nothing with the tree ");
            }
        }

        private void RemoveKey(int index)
        {
            _plantsReady[index].DestroyThisPlant();
            _plantsReady.Remove(index);
        }

        private void SetScoreExperience(int experience)
        {
            _scoreSystem.SetExperience(experience);
        }

        private void SetCarrotCount(int carrots)
        {
            _scoreSystem.SetCarrots(carrots);
        }

        private void UpdateUIPositionIcon()
        {
            foreach (KeyValuePair<PlantTile, UITimer> plant in _plants)
            {
                Vector2 targetPosition = _camera.WorldToScreenPoint(plant.Key.GetUITimerPosition().position);
                plant.Value.transform.position = Vector2.Lerp(plant.Value.transform.position, targetPosition, Time.deltaTime * _fps);
                plant.Value.TimeText.text = plant.Key.GetCurrentTime().ToString();
            }
        }

        public void SetScoreSystem(ScoreSystem system)
        {
            _scoreSystem = system;
        }
    }
}