using Cirrious.CrossCore;
using Cirrious.MvvmCross.Unity.Platform;
using Cirrious.MvvmCross.ViewModels;
using UnityEngine;

public class TestTutorialAppDelegate : MonoBehaviour
{
    void Start()
    {

        var setup = new TestTutorialSetup(this);
        setup.Initialize();

        var start = Mvx.Resolve<IMvxAppStart>();
        start.Start();

    }

}
