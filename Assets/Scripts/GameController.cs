using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MultimeterController multimeter;

        private void Start()
        {
            Instantiate(multimeter);
        }
    }
}
