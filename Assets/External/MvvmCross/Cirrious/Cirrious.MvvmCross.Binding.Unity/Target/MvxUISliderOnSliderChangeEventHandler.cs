using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Target
{
    public class MvxUISliderOnSliderChangeEventHandler : MonoBehaviour
    {
        public delegate void OnValueChange(float val);
        public OnValueChange onValueChange;

        public void OnSliderChange(float val)
        {
            // Notify the delegate
            if (onValueChange != null) onValueChange(val);
        }
    }
}
