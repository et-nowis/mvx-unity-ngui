// INotifyDataErrorInfo.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;

namespace System.ComponentModel
{
    public interface INotifyDataErrorInfo
    {
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        IEnumerable GetErrors(string propertyName);

        bool HasErrors { get; }
    }
}