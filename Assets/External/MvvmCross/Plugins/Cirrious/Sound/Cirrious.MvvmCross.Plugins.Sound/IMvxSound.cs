// IMvxSoundEffectInstance.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.Sound
{
    public interface IMvxSound
        : IDisposable
    {
		void PlayMusicPlaylist();
		void PlayMusic(string audioID);
		void StopMusic();
        void Play(string audioID);
        void Stop(string audioID);
	  	float MusicVolume { get; set; }
		float SFXVolume { get; set; }
    }
}