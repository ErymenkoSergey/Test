using Test.LavaProject.Farm.Mechanica_Spawner;
using Test.LavaProject.Farm.Mechanica.Score;
using Test.LavaProject.Farm.Mechanica_Spawner.Cells.Plants;
using Test.LavaProject.Farm.DI;
using Test.LavaProject.Farm.UI;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;


namespace Test.LavaProject.Farm.Mechanica_UI
{
    public class UIPresentor : MonoBehaviour
    {
        private SettingGame _settings;
        [Inject] private ScoreSystem _scoreSystem;
        private Spawner _spawner;
        private Camera _camera;

        [SerializeField] private GameObject _timerPrefab;
        [SerializeField] private RectTransform _fieldVisualize;

        private Dictionary<PlantTile, UITimer> _plants = new Dictionary<PlantTile, UITimer>();
        private Dictionary<int, PlantTile> _plantsReady = new Dictionary<int, PlantTile>();

        private float _fps = 60f;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settings = settingGame;
            _settings.UIPresentor = this;
        }

        private void Awake()
        {
            SetLinks();
        }

        private void SetLinks()
        {
            _camera = _settings.CameraControl.GetCamera();
            _spawner = _settings.Spawner;
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
            _spawner.SetStatusReady(index);
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
    }
}