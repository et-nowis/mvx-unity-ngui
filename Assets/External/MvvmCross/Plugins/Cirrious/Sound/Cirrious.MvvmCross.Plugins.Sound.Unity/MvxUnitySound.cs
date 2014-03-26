// MvxSound.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using UnityEngine;

namespace Cirrious.MvvmCross.Plugins.Sound.Unity
{
    public sealed class MvxUnitySound : IMvxSound
    {
        public MvxUnitySound()
        {
        }

        ~MvxUnitySound()
        {
            Dispose(false);
        }

        #region Implementation of IMvxSound
		
		public void PlayMusicPlaylist()
		{
			AudioController.PlayMusicPlaylist();	
		}
		
		public void PlayMusic(string audioID)
        {
            AudioController.PlayMusic(audioID);
        }
		
		public void StopMusic()
        {
            AudioController.StopMusic();
        }

        public void Play(string audioID)
        {
            AudioController.Play(audioID);
			
			
        }

        public void Stop(string audioID)
        {
			AudioController.Stop(audioID);
        }
		
		public float SFXVolume
		{
			get {
				return AudioController.GetCategoryVolume("SFX");
			}
			
			set {
				AudioController.SetCategoryVolume("SFX", value);	
			}
		}
		
		public float MusicVolume
		{
			get {
				return AudioController.GetCategoryVolume("Music");
			}
			
			set {
				AudioController.SetCategoryVolume("Music", value);	
			}
		}
		
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
            }
        }

        #endregion
    }
}