using Test.LavaProject.Farm.DI;
using Test.LavaProject.Farm.UI;
using Zenject;

namespace Test.LavaProject.Farm.Mechanica.Score
{
    public class ScoreSystem
    {
        private SettingGame _settingGame;
        private UIManager _uImanager;

        [Inject]
        public void Construct(SettingGame settingGame)
        {
            _settingGame = settingGame;

            SetUiManager();
        }

        private void SetUiManager()
        {
            _uImanager = _settingGame.UIManager;
        }

        private int _experience;
        private int _carrots;

        public void SetCarrots(int carrots)
        {
            _carrots += carrots;
            SetUiText(TypeScoreText.Carrots);
        }

        public void SetExperience(int experience)
        {
            _experience += experience;
            SetUiText(TypeScoreText.Experience);
        }

        private void SetUiText(TypeScoreText type)
        {
            if (type == TypeScoreText.Experience)
                _uImanager.SetTextExperience(_experience.ToString());
            if (type == TypeScoreText.Carrots)
                _uImanager.SetTextCarrots(_carrots.ToString());
        }

        public void OpenChoicePanel(bool isOpen)
        {
            _uImanager.OpenChoicePanel(isOpen);
        }

        private void DellData()
        {
            _experience = 0;
            _carrots = 0;
        }
    }
}