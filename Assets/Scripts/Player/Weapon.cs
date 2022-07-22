using Colors;
using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private ColorsController.Color projectileColor = 0;

        public void ChangeWeaponColor()
        {
            projectileColor = ColorsController.Instance.GetNextColor(projectileColor);
            Debug.Log(projectileColor);
        }
    }
}
