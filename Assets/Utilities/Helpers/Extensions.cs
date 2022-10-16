using UnityEngine;

namespace Racer.Utilities
{
    public static class Extensions
    {
        public static void ToggleActive(this GameObject gameObject, bool state)
        {
            if (state)
            {
                if (!gameObject.activeInHierarchy)
                    gameObject.SetActive(true);
            }
            else
            {
                if (gameObject.activeInHierarchy)
                    gameObject.SetActive(false);
            }
        }

        public static void IsEnabled(this Behaviour behaviour, bool state)
        {
            if (state)
            {
                if (!behaviour.enabled)
                    behaviour.enabled = true;
            }
            else
            {
                if (behaviour.enabled)
                    behaviour.enabled = false;
            }
        }
    }
}