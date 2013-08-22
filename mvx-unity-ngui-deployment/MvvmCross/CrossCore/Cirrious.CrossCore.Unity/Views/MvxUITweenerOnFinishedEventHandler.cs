using UnityEngine;

namespace Cirrious.CrossCore.Unity.Views
{
    public class MvxUITweenerOnFinishedEventHandler : MonoBehaviour
    {
        public delegate void OnFinished(UITweener tween);
        public OnFinished onFinished;

        public void OnTweenFinished(UITweener tween)
        {
            // Notify the delegate
            if (onFinished != null) onFinished(tween);
        }
    }
}
