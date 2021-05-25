using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SimPS.Components
{
    class SPS
    {
        //Properties
        public PinsContainer Pins = new PinsContainer();
        //TODO: Actions

        public class PinsContainer
        {
            //Motor_Auf
            public Pin A0 = new Pin(Pin.StandartPinPosition.Opener, Pin.Pinmode.Output);
            //Motor_Zu
            public Pin A1 = new Pin(Pin.StandartPinPosition.Opener, Pin.Pinmode.Output);

            //ES_oben
            public Pin E0 = new Pin(Pin.StandartPinPosition.Opener, Pin.Pinmode.Input);
            //ES_unten
            public Pin E1 = new Pin(Pin.StandartPinPosition.Opener, Pin.Pinmode.Input);
            //Taster_Auf
            public Pin E2 = new Pin(Pin.StandartPinPosition.Shutter, Pin.Pinmode.Input);
            //Taster_Zu
            public Pin E3 = new Pin(Pin.StandartPinPosition.Shutter, Pin.Pinmode.Input);
            //Taster_Stop
            public Pin E4 = new Pin(Pin.StandartPinPosition.Opener, Pin.Pinmode.Input);
            //LS_Unterbrochen
            public Pin E5 = new Pin(Pin.StandartPinPosition.Opener, Pin.Pinmode.Input);
        }

        public class Pin
        {
            public enum StandartPinPosition { Shutter = 0, Opener = 1 }
            public enum Pinmode { Output = 0, Input = 1 };

            public Pinmode Mode;
            public readonly uint Id;
            public bool Value
            {
                get
                {
                    return _Value;
                }
                set
                {
                    PinChangedEventHandler handler = PinChanged;
                    handler?.Invoke(this, new PinChangedEventArgs(this, value, this.Value));
                    _Value = value;
                }
            }
            public bool StandartPosition;

            public static Dictionary<Keys, Pin> KeyMap = new Dictionary<Keys, Pin>();

            public static event PinChangedEventHandler PinChanged;
            public delegate void PinChangedEventHandler(Object sender, PinChangedEventArgs e);
            public class PinChangedEventArgs : EventArgs
            {
                public PinChangedEventArgs(Pin pin, bool newVal, bool oldVal)
                {
                    this.Pin = pin;
                    this.NewValue = newVal;
                    this.OldValue = oldVal;
                }
                public Pin Pin;
                public bool OldValue;
                public bool NewValue;
            }

            public Keys MappedKey
            {
                get
                {
                    return KeyMap.FirstOrDefault(x => x.Value == this).Key;
                }
                set
                {
                    foreach (var pin in KeyMap)
                    {
                        if (pin.Value.Id == this.Id) return;
                    }

                    KeyMap.Add(value, this);
                }
            }

            private static uint CurrKey;
            private bool _Value;

            public bool isPressed()
            {
                if (this.Value == !this.StandartPosition) return true;
                else return false;
            }

            public Pin(StandartPinPosition stdPos, Pinmode mode)
            {
                CurrKey += 1;
                Id = CurrKey;
                if (stdPos == StandartPinPosition.Opener) StandartPosition = true;
                else StandartPosition = false;
                Value = StandartPosition;
                Mode = mode;
            }
        }
    }
}
