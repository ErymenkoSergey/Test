using Test.LavaProject.Farm.Data;
using Test.LavaProject.Farm.DI;
using Test.LavaProject.Farm.Mechanica.Score;
using Test.LavaProject.Farm.Mechanica_Input;
using Test.LavaProject.Farm.Mechanica_Spawner;
using Test.LavaProject.Farm.Mechanica_Spawner.Planting;
using Test.LavaProject.Farm.Mechanica_UI;
using UnityEngine;
using Zenject;

namespace Test.LavaProject.Farm.Mechanica.MainProcess.Game
{
    public class GameProcess : MonoBehaviour
    {
        [SerializeField] private GameConfiguration _gameConfig;
        [Inject] private ScoreSystem _scoreSystem;

        private SettingGame _settings;
        private UIPresentor _uIPresentor;
        private Spawner _spawner;
        private TouchCheck _touchCheck;
        private CameraControl _cameraControl;
        private PlantingSystem _plantingSystem;

        private int _currentCell;
        private PlantType _selectedType;
        private Transform _selectedTransform;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settings = settingGame;
            _settings.GameProcess = this;
        }

        private void Awake()
        {
            _spawner = _settings.Spawner;
            _cameraControl = _settings.CameraControl;
            _plantingSystem = _settings.PlantingSystem;
            _uIPresentor = _settings.UIPresentor;
        }

        private void Start()
        {
            _spawner.SetGameConfiguration(_gameConfig.CellSize);
            SetDataPlantingSystem();
        }

        private void SetDataPlantingSystem()
        {
            _plantingSystem.SetData(_gameConfig.Plants);
        }

        public void SetTouchCheck(TouchCheck touchCheck)
        {
            _touchCheck = touchCheck;
        }

        public void SetSelectedCell(int index)
        {
            _currentCell = index;
            _scoreSystem.OpenChoicePanel(true);
        }

        public void Harvesting(int index)
        {
            _currentCell = index;
            _spawner.CheckStatusReady(index);
        }

        public void StartPlanting(PlantType type)
        {
            _spawner.StartPlanting(_currentCell, out Transform point);
            _selectedType = type;
            _selectedTransform = point;
        }

        public void CancelPlanting()
        {
            _currentCell = 0;
            _touchCheck.SetChecngeChoiseStatus(false);
        }

        public void SetTransformFarmer(Transform farmerPos)
        {
            _cameraControl.SetGameObject(farmerPos);
        }

        public void StartDisembarkation(int selectedCell, out PlantType type)
        {
            type = _selectedType;
            SetChecngeChoiseStatus(false);
            _plantingSystem.CreateNewPlant(_selectedType, _selectedTransform, selectedCell);
            _cameraControl.CameraZoomCalculation(_selectedTransform.position);
        }

        public void PickUpHarvest(int cell)
        {
            SetChecngeChoiseStatus(false);
            _uIPresentor.ReadinessCheck(cell);
        }

        private void SetChecngeChoiseStatus(bool isEnable)
        {
            _touchCheck.SetChecngeChoiseStatus(isEnable);
        }

        public Texture2D GetIconPlant(PlantType plant)
        {
            Texture2D texture = null;

            for (int i = 0; i < _gameConfig.Plants.Length; i++)
            {
                if (_gameConfig.Plants[i].PlantType == plant)
                {
                    texture = _gameConfig.Plants[i].UIIcon;
                    return texture;
                }
            }

            return texture;
        }
    }
}

public delegate Texture2D GetImageUI(PlantType plant);