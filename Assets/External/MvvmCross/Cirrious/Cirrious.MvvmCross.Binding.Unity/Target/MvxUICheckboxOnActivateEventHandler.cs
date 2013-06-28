using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Target
{
    public class MvxUICheckboxOnActivateEventHandler : MonoBehaviour
    {
        public delegate void OnStateChange(bool state);
        public OnStateChange onStateChange;

        public void OnActivate(bool state)
        {
            // Notify the delegate
            if (onStateChange != null) onStateChange(state);
        }
    }
}
