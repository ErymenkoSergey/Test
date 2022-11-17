using Test.LavaProject.Farm.UI;

namespace Test.LavaProject.Farm.Mechanica.Score
{
    public class ScoreSystem
    {
        private UIManager _uImanager;

        public ScoreSystem(UIManager uIManager)
        {
            _uImanager = uIManager;
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
    }
}