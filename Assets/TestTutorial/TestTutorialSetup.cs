using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Unity.Platform;
using Cirrious.MvvmCross.Unity.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;

public class TestTutorialSetup
    : MvxUnitySetup
{

    public TestTutorialSetup(MvxApplicationDelegate applicationDelegate)
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
        //registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.Location.Touch.Plugin>();
        //registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.ThreadUtils.Touch.Plugin>();
        //registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.File.Touch.Plugin>();
        //registry.AddConventionalPlugin<Cirrious.MvvmCross.Plugins.DownloadCache.Touch.Plugin>();
        base.AddPluginsLoaders(registry);
    }

    protected override void InitializeLastChance()
    {
        base.InitializeLastChance();
        //Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
        //Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
        Cirrious.MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();
    }
    #endregion
}

