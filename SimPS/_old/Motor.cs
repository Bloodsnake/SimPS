using System;
using System.Timers;

namespace SimPS._old.Components
{
    class Motor
    {
        public Motor()
        {
            timer.Interval = 500;
            timer.Elapsed += (e, a) =>
            {
                if (Mode == MotorMode.Up) Percentage += 1;
                else if (Mode == MotorMode.Down) Percentage -= 1;
            };
            timer.Start();
        }

        public enum MotorMode { Stand = 0, Up = 1, Down = 2 };

        public MotorMode Mode = MotorMode.Stand;
        private int _Percentage;

        private readonly Timer timer = new Timer();

        public int Percentage 
        { 
            get 
            {
                return _Percentage;
            } 
            set 
            {
                if (value > 100 || value < 0)
                {
                    Mode = MotorMode.Stand;
                    return;
                }
                else _Percentage = value;
            }
        }
        public void MoveUp()
        {
            Mode = MotorMode.Up;
        }

        public void MoveDown()
        {
            Mode = MotorMode.Down;
        }
    }
}
