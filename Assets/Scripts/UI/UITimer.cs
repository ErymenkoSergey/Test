using UnityEngine;
using TMPro;

namespace Test.LavaProject.Farm.UI
{
    public class UITimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeText;
        public TextMeshProUGUI TimeText => _timeText;
    }
}