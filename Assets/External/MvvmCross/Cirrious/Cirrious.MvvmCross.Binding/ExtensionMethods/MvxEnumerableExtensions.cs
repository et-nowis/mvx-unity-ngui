// MvxEnumerableExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections;

namespace Cirrious.MvvmCross.Binding.ExtensionMethods
{
    public static class MvxEnumerableExtensions
    {
        public static int Count(this IEnumerable enumerable)
        {
            if (enumerable == null)
                return 0;

            var itemsList = enumerable as ICollection;
            if (itemsList != null)
            {
                return itemsList.Count;
            }

            var enumerator = enumerable.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }

        public static int GetPosition(this IEnumerable items, object item)
        {
            if (items == null)
            {
                return -1;
            }

            var itemsList = items as IList;
            if (itemsList != null)
            {
                {
                    return itemsList.IndexOf(item);
                }
            }

            var enumerator = items.GetEnumerator();
            for (var i = 0; ; i++)
            {
                if (!enumerator.MoveNext())
                {
                    return -1;
                }

                if (enumerator.Current == null)
                {
                    if (item == null)
                        return i;
                }
                // Note: do *not* use == here - see https://github.com/slodge/MvvmCross/issues/309
                else if (enumerator.Current.Equals(item))
                {
                    return i;
                }
            }
        }

#if UNITY_IPHONE
        private static System.Type iListType = typeof(IList);
        private static System.Reflection.MethodInfo getItemMethodInfo = typeof(IList).GetMethod("get_Item");
        public static System.Object ElementAt(this IEnumerable items, int position)
        {
            if (items == null)
                return null;

            var interfaces = items.GetType().GetInterfaces();
            bool isIList = false;
            foreach (var x in interfaces)
            {
                if (x == iListType)
                {
                    isIList = true;
                    break;
                }
            }
            if (isIList)
            {
                try
                {
                    return getItemMethodInfo.Invoke(items, new System.Object[] { position });
                }
                catch (System.Exception)
                {
                }
            }

            var enumerator = items.GetEnumerator();
            for (var i = 0; i <= position; i++)
            {
                enumerator.MoveNext();
            }
            return enumerator.Current;
        }
#else
        public static System.Object ElementAt(this IEnumerable items, int position)
        {
            if (items == null)
                return null;

            var itemsList = items as IList;
            if (itemsList != null)
            {
                return itemsList[position];
            }

            var enumerator = items.GetEnumerator();
            for (var i = 0; i <= position; i++)
            {
                enumerator.MoveNext();
            }

            return enumerator.Current;
        }
#endif

    }
}
