// MvxCanCreateTouchViewExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Unity.Views
{
    public static class MvxCanCreateUnityViewExtensionMethods
    {
        public static IMvxUnityView CreateViewControllerFor<TTargetViewModel>(this IMvxCanCreateUnityView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject == null
                                                                   ? null
                                                                   : parameterObject.ToSimplePropertyDictionary());
        }

#warning TODO - could this move down to IMvxView level?
        public static IMvxUnityView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateUnityView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            var parameterBundle = new MvxBundle(parameterValues);
            var request = new MvxViewModelRequest<TTargetViewModel>(parameterBundle, null,
                                                                    MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxUnityView CreateViewControllerFor<TTargetViewModel>(
            this IMvxCanCreateUnityView view,
            MvxViewModelRequest request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.Resolve<IMvxUnityViewCreator>().CreateView(request);
        }

        public static IMvxUnityView CreateViewControllerFor(
            this IMvxCanCreateUnityView view,
            MvxViewModelRequest request)
        {
            return Mvx.Resolve<IMvxUnityViewCreator>().CreateView(request);
        }

        public static IMvxUnityView CreateViewControllerFor(
            this IMvxCanCreateUnityView view,
            IMvxViewModel viewModel)
        {
            return Mvx.Resolve<IMvxUnityViewCreator>().CreateView(viewModel);
        }
    }
}