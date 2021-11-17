using System;
using HoneyPot.DebugUtil;
using HoneyPot.Helper;
using HoneyPot.Scene.Helper;

namespace HoneyPot.Scene.Steps
{
    class UndressStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;

        public DressType DressType { get; }

        public UndressStep(DressType dressType)
        {
            DressType = dressType;
        }

        public void InvokeStep()
        {
            switch (DressType)
            {
                case DressType.Full:
                    DebugLog.Instance.AddError("DressType.Full is currently not supported! You have to use ShowGirl step atm.");
                    break;
                case DressType.FullWithBra:
                    GirlHelper.WithBra();
                    break;
                case DressType.Underwear:
                    GirlHelper.Underwear();
                    break;
                case DressType.BraOnly:
                    GirlHelper.OnlyBra();
                    break;
                case DressType.PantiesOnly:
                    GirlHelper.OnlyPanties();
                    break;
                case DressType.Nude:
                    GirlHelper.Nude();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            StepFinished?.Invoke();
        }
    }
}