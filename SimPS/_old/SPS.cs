using System;
using System.Collections.Generic;
using System.Timers;

namespace SimPS._old.Components
{
    class SPS
    {
        #region Properties

        public PinsContainer Pins { get; set; } = new PinsContainer();

        public Action LoadedSetup { get; set; }

        public Action LoadedLoop { get; set; }

        #endregion

        #region Functions
        /* Legacy, war eine schlechte Idee.
         * Ist nur hier weils trotzdem cool ist :3
        private Task StartInputs()
        {
            ConsoleKey oldKey = new ConsoleKey();
            var timer = new Timer();
            var clock = new Stopwatch();
            clock.Start();
            uint releaseTime = 100;             //Time it takes until key is registert as released
            ConsoleKey pressedKey = new ConsoleKey();
            //Clock
            while (true)
            {
                if (clock.ElapsedMilliseconds > releaseTime)
                {
                    //If no new key is registert: KEY GOT RELEASED
                    if (Pins.KeyMap.ContainsKey(pressedKey)) Pins.KeyMap[pressedKey].Value = false;
                    clock.Restart();
                    releaseTime = 100;
                }
                //If key can't be read
                if (Console.KeyAvailable == false) continue;
                //New key can be registert
                //New key get's registert, Clock gets reset
                clock.Restart();
                //Get new Key
                ConsoleKey newKey = Console.ReadKey(true).Key;
                //If the key is still pressed
                if (newKey == oldKey)
                {
                    //KEY IS STILL PRESSED / IS PRESSED

                    if (Pins.KeyMap.ContainsKey(newKey)) Pins.KeyMap[newKey].Value = true;
                    pressedKey = newKey;
                }
                //ELSE NO KEY IS PRESSED
                else
                {
                }
                releaseTime = 500;
                oldKey = newKey;
            }
        
        }
        */
        public static void WireOutput(Pin pin, Action function)
        {
            if (pin.Mode == Pin.Pinmode.Input) throw new InputNotSupportedException();
            else pin.Function = function;
        }
        //Functions to emulate the Arduino "digitalWrite() and digitalRead() functions"
        public static bool ReadPin(Pin pin)
        {
            return pin.Value;
        }

        public static void WritePin(Pin pin, bool value)
        {
            pin.Value = value;
        }

        //Load setup and loop
        public void LoadProgram(Action setup, Action loop)
        {
            //StartSimulation gets executed async, to not block the main thread with IO
            LoadedSetup = setup;
            LoadedLoop = loop;

            setup();
            while (true)
            {
                loop();
                //Output.Draw();
            }
        }

        #endregion

        #region Pins
        /// <summary>
        // Stores all Pins of the SPS
        /// </summary>
        public class PinsContainer
        {
            internal PinsContainer() 
            {
                //Motor_Auf
                A0 = new Pin("A0", Pin.Pinmode.Output, this, Pin.PinPosition.Opener);
                //Motor_Zu
                A1 = new Pin("A1", Pin.Pinmode.Output, this, Pin.PinPosition.Opener);

                //ES_oben
                E0 = new Pin("E0", Pin.Pinmode.Input, this, Pin.PinPosition.Opener);
                //ES_unten
                E1 = new Pin("E1", Pin.Pinmode.Input, this, Pin.PinPosition.Opener);
                //Taster_Auf
                E2 = new Pin("E2", Pin.Pinmode.Input, this, Pin.PinPosition.Shutter);
                //Taster_Zu
                E3 = new Pin("E3", Pin.Pinmode.Input, this, Pin.PinPosition.Shutter);
                //Taster_Stop
                E4 = new Pin("E4", Pin.Pinmode.Input, this, Pin.PinPosition.Opener);
                //LS_Unterbrochen
                E5 = new Pin("E5", Pin.Pinmode.Input, this, Pin.PinPosition.Opener);
            }
            public Dictionary<ConsoleKey, Pin> KeyMap = new Dictionary<ConsoleKey, Pin>();

            #region Individual Pins

            public readonly Pin A0;

            public readonly Pin A1;

            public readonly Pin E0;

            public readonly Pin E1;

            public readonly Pin E2;

            public readonly Pin E3;

            public readonly Pin E4;

            public readonly Pin E5;

            #endregion
        }
        #region Pin
        /// <summary>
        // Stores all information of a Pin
        /// </summary>
        public class Pin
        {
            private Timer ReleaseTimer = new Timer();
            public void Press()
            {

            }
            private bool StandartPosition;
            private readonly PinsContainer Container;
            private ConsoleKey _MappedKey;
            public ConsoleKey MappedKey
            {
                get { return _MappedKey; }
                set
                {
                    if (_MappedKey != value)
                    {
                        foreach (var entry in Container.KeyMap)
                        {
                            if (entry.Value.Id == this.Id)
                            {
                                if (entry.Key == value) throw new KeyAlreadyMappedException();
                                Container.KeyMap.Remove(entry.Key);
                            }
                        }
                        Container.KeyMap.Add(value, this);
                    }
                    _MappedKey = value;
                }
            }
            public PinPosition PinPostition;
            public Pinmode Mode;
            public Action Function;

            public void SetFunction(Action function)
            {
                this.Function = function;
            }

            public readonly string Id;
            private bool _value;
            public bool Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    //If the pin gets set the function gets executed
                    if (Function != null)
                    {
                        this.Function();
                    }
                    _value = value;
                }
            }
            public enum PinPosition { Opener = 1, Shutter = 2}
            public enum Pinmode { Output = 0, Input = 1 };
            public Pin(string id, Pinmode mode, PinsContainer container, PinPosition pos)
            {
                if (pos == PinPosition.Opener)
                {
                    //Standart Öffner
                    StandartPosition = false;
                }
                else if (pos == PinPosition.Shutter)
                {
                    //Standart Schließer
                    StandartPosition = true;
                }
                ReleaseTimer.Interval = 100;
                this.Container = container;
                this.Id = id;
                this.Mode = mode;
            }
            private class KeyAlreadyMappedException : Exception
            {
                public KeyAlreadyMappedException() : base("Key is already mapped to another Pin") { }
            }
        }
        #endregion
        #endregion

        #region Exceptions

        private class InputNotSupportedException : Exception {
            public InputNotSupportedException() :base("Inputs can't execute a function after changing value") { }
        }
        private class SimulationAlreadyStartedException : Exception {
            public SimulationAlreadyStartedException() : base("Simulation already started") { }
        }
        #endregion
    }
}