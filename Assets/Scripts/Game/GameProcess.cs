using Test.LavaProject.Farm.Data;
using Test.LavaProject.Farm.Mechanica.Score;
using Test.LavaProject.Farm.Mechanica_Input;
using UnityEngine;

namespace Test.LavaProject.Farm.Mechanica.MainProcess.Game
{
    public class GameProcess : MonoBehaviour, IGameProcess
    {
        [SerializeField] private GameConfiguration _gameConfig;
        private ScoreSystem _scoreSystem;
        [SerializeField] private GameObject _uIPresentor;
        private IUIPresentor _uiPresentor;
        [SerializeField] private GameObject _spawner;
        private ISpawn _iSpawn;
        private TouchCheck _touchCheck;
        [SerializeField] private GameObject _cameraControl;
        private ICameraControl _iCameraControl;
        [SerializeField] private GameObject _plantingSystem;
        private IPlantingSystem _iPlantingSystem;
        private int _currentCell;
        private PlantType _selectedType;
        private Transform _selectedTransform;

        private void Awake()
        {
            SetLinks();
        }

        private void SetLinks()
        {
            if (_cameraControl.TryGetComponent(out ICameraControl camera)) ;
            _iCameraControl = camera;

            if (_spawner.TryGetComponent(out ISpawn spawn))
                _iSpawn = spawn;

            if (_uIPresentor.TryGetComponent(out IUIPresentor presentor))
                _uiPresentor = presentor;

            if (_plantingSystem.TryGetComponent(out IPlantingSystem planting))
                _iPlantingSystem = planting;

            if (_uiPresentor == null)
                Debug.Log("_uiPresentor null");
        }

        private void Start()
        {
            _iSpawn.SetGameConfiguration(_gameConfig.CellSize);
            SetDataPlantingSystem();
        }

        private void SetDataPlantingSystem()
        {
            _iPlantingSystem.SetData(_gameConfig.Plants);
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

        public void SetScoreSystem(ScoreSystem system)
        {
            _scoreSystem = system;
            _uiPresentor.SetScoreSystem(system);
        }

        public void Harvesting(int index)
        {
            _currentCell = index;
            _iSpawn.CheckStatusReady(index);
        }

        public void StartPlanting(PlantType type)
        {
            _iSpawn.StartPlanting(_currentCell, out Transform point);
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
            _iCameraControl.SetGameObject(farmerPos);
        }

        public void StartDisembarkation(int selectedCell, out PlantType type)
        {
            type = _selectedType;
            SetChecngeChoiseStatus(false);
            _iPlantingSystem.CreateNewPlant(_selectedType, _selectedTransform, selectedCell);
            _iCameraControl.CameraZoomCalculation(_selectedTransform.position);
        }

        public void PickUpHarvest(int cell)
        {
            SetChecngeChoiseStatus(false);
            _uiPresentor.ReadinessCheck(cell);
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

        public IUIPresentor GetUIPresentor()
        {
            return _uiPresentor;
        }
    }
}

public delegate Texture2D GetImageUI(PlantType plant);