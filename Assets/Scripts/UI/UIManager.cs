using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Test.LavaProject.Farm.Mechanica.Score;

namespace Test.LavaProject.Farm.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject _gameProcess;
        private IGameProcess _iGameProcess;

        private ScoreSystem _scoreSystem;

        [SerializeField] private GameObject _choicePanel;

        [SerializeField] private TextMeshProUGUI _textExperience;
        [SerializeField] private TextMeshProUGUI _textCarrots;

        [SerializeField] private Button _closeChoicePanelButton;

        [SerializeField] private Button _choiceGrassButton;
        [SerializeField] private Button _choiceCarrotsButton;
        [SerializeField] private Button _choiceTreeButton;

        [SerializeField] private RawImage _grassImage;
        [SerializeField] private RawImage _carrotsImage;
        [SerializeField] private RawImage _treeImage;

        private void OnEnable()
        {
            _closeChoicePanelButton.onClick.AddListener(CloseChoicePanel);

            _choiceGrassButton.onClick.AddListener(() => ChoicePlant(PlantType.Grass));
            _choiceCarrotsButton.onClick.AddListener(() => ChoicePlant(PlantType.Carrot));
            _choiceTreeButton.onClick.AddListener(() => ChoicePlant(PlantType.Tree));

            if (_gameProcess.TryGetComponent(out IGameProcess process))
                _iGameProcess = process;


        }

        private void Start()
        {
            CreateScoreSystem();
            SetImageIcons();
            SetScoreSystem();
        }

        private void CreateScoreSystem()
        {
            _scoreSystem = new ScoreSystem(this);
        }

        private void OnDisable()
        {
            _closeChoicePanelButton.onClick.RemoveListener(CloseChoicePanel);

            _choiceGrassButton.onClick.RemoveListener(() => ChoicePlant(PlantType.Grass));
            _choiceCarrotsButton.onClick.RemoveListener(() => ChoicePlant(PlantType.Carrot));
            _choiceTreeButton.onClick.RemoveListener(() => ChoicePlant(PlantType.Tree));
        }

        public void SetTextExperience(string countExperience)
        {
            _textExperience.text = $"Experience: {countExperience}";
        }

        public void SetTextCarrots(string countCarrots)
        {
            _textCarrots.text = $"Carrots: {countCarrots}";
        }

        private void ChoicePlant(PlantType type)
        {
            OpenChoicePanel(false);
            _iGameProcess.StartPlanting(type);
        }

        public void OpenChoicePanel(bool isOpen)
        {
            _choicePanel.SetActive(isOpen);
        }

        private void CloseChoicePanel()
        {
            OpenChoicePanel(false);
            _iGameProcess.CancelPlanting();
        }

        private void SetImageIcons()
        {
            GetImageUI imageUI;
            imageUI = _iGameProcess.GetIconPlant;

            _grassImage.texture = imageUI(PlantType.Grass);
            _carrotsImage.texture = imageUI(PlantType.Carrot);
            _treeImage.texture = imageUI(PlantType.Tree);
        }

        public ScoreSystem GetScoreSystem()
        {
            return _scoreSystem;
        }

        private void SetScoreSystem()
        {
            _iGameProcess.SetScoreSystem(_scoreSystem);
        }
    }
}