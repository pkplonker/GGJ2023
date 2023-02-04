using TMPro;
using UnityEngine;

namespace Stuart
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private int id;
        private Inventory invent;
        [SerializeField] private TextMeshProUGUI nutText;
        [SerializeField] private TextMeshProUGUI waterText;
        [SerializeField] private TextMeshProUGUI sproutText;

        private void Awake()
        {
            var invents = FindObjectsOfType<Inventory>();
            foreach (var i in invents)
            {
                if (i.playerId != id) continue;
                invent = i;
                invent.OnInventChanged += InventChanged;
                break;
            }
        }

        private void InventChanged()
        {
            nutText.text = ((int)invent.GetResource(Resource.Nutrients)).ToString();
            waterText.text = ((int)invent.GetResource(Resource.Water)).ToString();
            sproutText.text = ((int)invent.GetResource(Resource.Sprout)).ToString();

        }

        private void OnDisable()
        {
            if (invent) invent.OnInventChanged += InventChanged;
        }
    }
}