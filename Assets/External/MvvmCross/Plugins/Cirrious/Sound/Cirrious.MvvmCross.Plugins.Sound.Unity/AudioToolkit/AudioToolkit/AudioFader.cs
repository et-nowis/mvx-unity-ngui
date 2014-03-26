using UnityEngine;
using System.Collections;

#pragma warning disable 1591 // undocumented XML code warning

public class AudioFader 
{
    float _fadeOutTotalTime = -1;
    double _fadeOutStartTime = -1;

    float _fadeInTotalTime = -1;
    double _fadeInStartTime = -1;

    public double time
    {
        get;
        set;
    }

    public bool isFadingOut
    {
        get
        {
            return _fadeOutTotalTime >= 0 && time > _fadeOutStartTime;
        }
    }

    public bool isFadingIn
    {
        get
        {
            return _fadeInTotalTime > 0;
        }
    }

    public void Set0()
    {
        time = 0;

        _fadeOutTotalTime = -1;
        _fadeOutStartTime = -1;

        _fadeInTotalTime = -1;
        _fadeInStartTime = -1;
    }

    public void FadeIn( float fadeInTime )
    {
        FadeIn( fadeInTime, time );
    }

    public void FadeIn( float fadeInTime, double startToFadeTime )
    {
        _fadeInTotalTime = fadeInTime;
        _fadeInStartTime = startToFadeTime;
    }

    public void FadeOut( float fadeOutLength, float startToFadeTime )
    {
        if ( isFadingOut )
        {
            double requestedEndOfFadeout = time + startToFadeTime + fadeOutLength;
            double currentEndOfFadeout = _fadeOutStartTime + _fadeOutTotalTime;

            if ( currentEndOfFadeout < requestedEndOfFadeout )
            {
                // current fade-out is already faster than the requested fade-out
                return;
            }
            else
            {
                // combine the two fade-outs
                double alreadyFadedTime = time - _fadeOutStartTime;
                double timeToFinishFade = startToFadeTime + fadeOutLength;

                double currentTimeToFinishFade = currentEndOfFadeout - time;

                double newFadedTime;

                if ( currentTimeToFinishFade != 0 )
                {
                    newFadedTime = alreadyFadedTime * timeToFinishFade / currentTimeToFinishFade;

                    _fadeOutStartTime = time - newFadedTime;
                    _fadeOutTotalTime = (float) ( timeToFinishFade + newFadedTime );
                }
            }

        }
        else
        {
            _fadeOutTotalTime = fadeOutLength;
            _fadeOutStartTime = time + startToFadeTime;
        }
    }

    public float Get( out bool finishedFadeOut )
    {
        float fadeVolume = 1;

        finishedFadeOut = false;

        if ( isFadingOut )
        {
            fadeVolume *= 1.0f - _GetFadeValue( (float) ( time - _fadeOutStartTime ), _fadeOutTotalTime );

            if ( fadeVolume == 0 )
            {
                finishedFadeOut = true; 
                return 0;
            }
        }

        if ( isFadingIn )
        {
            fadeVolume *= _GetFadeValue( (float) ( time - _fadeInStartTime ), _fadeInTotalTime );
        }
        return fadeVolume;
    }

    private float _GetFadeValue( float t, float dt )
    {
        if ( dt <= 0 )
        {
            return t > 0 ? 1.0f : 0;
        }
        return Mathf.Clamp( t / dt, 0.0f, 1.0f );
    }
}
