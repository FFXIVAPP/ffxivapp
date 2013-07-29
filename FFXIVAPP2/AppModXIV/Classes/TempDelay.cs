// AppModXIV
// TempDelay.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace AppModXIV.Classes
{
    public class TempDelay
    {
        public TempDelay(int tempDelay)
        {
            _tempDelay = tempDelay/1000;
        }

        public void Set_Delay(int setDelay)
        {
            _timeDelaySet = ElapsedSec();
            _setDelay = setDelay/1000;
        }

        public Boolean DelayOver()
        {
            return ElapsedSec() > _timeDelaySet + _setDelay;
        }

        private float ElapsedSec()
        {
            if (DateTime.Now.Hour*3600 + DateTime.Now.Minute*60 + DateTime.Now.Second + DateTime.Now.Millisecond/1000 < _startTime)
            {
                _startTime = _startTime - 86400;
            }
            return DateTime.Now.Hour*3600 + DateTime.Now.Minute*60 + DateTime.Now.Second + DateTime.Now.Millisecond/1000 - _startTime;
        }

        private float _startTime = DateTime.Now.Hour*3600 + DateTime.Now.Minute*60 + DateTime.Now.Second;
        private float _tempDelay;
        private float _timeDelaySet;
        private float _setDelay;
    }
}