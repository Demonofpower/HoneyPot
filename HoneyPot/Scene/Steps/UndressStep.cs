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
        public bool AltGirlDressChange { get; }

        public UndressStep(DressType dressType, bool altGirlDressChange)
        {
            DressType = dressType;
            AltGirlDressChange = altGirlDressChange;
        }

        public void InvokeStep()
        {
            switch (DressType)
            {
                case DressType.Full:
                    DebugLog.Instance.AddError("DressType.Full is currently not supported! You have to use ShowGirl step atm.");
                    break;
                case DressType.FullWithBra:
                    GirlHelper.WithBra(AltGirlDressChange);
                    break;
                case DressType.Underwear:
                    GirlHelper.Underwear(AltGirlDressChange);
                    break;
                case DressType.BraOnly:
                    GirlHelper.OnlyBra(AltGirlDressChange);
                    break;
                case DressType.PantiesOnly:
                    GirlHelper.OnlyPanties(AltGirlDressChange);
                    break;
                case DressType.Nude:
                    GirlHelper.Nude(AltGirlDressChange);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            StepFinished?.Invoke();
        }
    }
}