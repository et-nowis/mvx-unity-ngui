using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Unity.Platform;
using Cirrious.MvvmCross.Unity.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using UnityEngine;

public class TestTutorialSetup
    : MvxUnitySetup
{

    public TestTutorialSetup(MonoBehaviour applicationDelegate)
        : base(applicationDelegate)
    {
    }

    #region Overrides of MvxBaseSetup

    protected override IMvxApplication CreateApp()
    {
        var app = new TestTutorialApp();
        return app;
    }

    protected override void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
    {
        //registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Location.Unity.Plugin>();
		//registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ThreadUtils.Unity.Plugin>();
		registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.Unity.Plugin>();
		registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.DownloadCache.Unity.Plugin>();
		registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ResourceLoader.Unity.Plugin>();
        base.AddPluginsLoaders(registry);
    }

    protected override void InitializeLastChance()
    {
        base.InitializeLastChance();
        Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
        Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
		Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();
        Cirrious.MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();
    }
    #endregion
}

