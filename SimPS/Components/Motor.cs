using SimPS.Simulation;
using System;
using System.Timers;

namespace SimPS.Components
{
    class Motor
    {
        private readonly MotorContext MX;
        public Motor(MotorContext mx)
        {
            MX = mx;
            timer.Interval = 500;
            timer.Elapsed += (o, a) =>
            {
                if (Mode == MotorMode.Up) Percentage += 1;
                else if (Mode == MotorMode.Down) Percentage -= 1;

                if (Percentage <= 0)
                {
                    Percentage = 0;
                    MX.EndBottomPin.Value = !MX.EndBottomPin.StandartPosition;
                }
                else if (Percentage >= 100)
                {
                    Percentage = 100;
                    MX.EndTopPin.Value = !MX.EndTopPin.StandartPosition;
                }
                else
                {
                    MX.EndBottomPin.Value = MX.EndBottomPin.StandartPosition;
                    MX.EndTopPin.Value = MX.EndTopPin.StandartPosition;
                }
                if (Mode == MotorMode.EmergencyStop) Output.Draw(Percentage, ConsoleColor.Red);
                else Output.Draw(Percentage);
            };
            timer.Start();
        }

        public enum MotorMode { Stand = 0, Up = 1, Down = 2, EmergencyStop = 64 };

        private bool isStopped = false;

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
        public void Stop()
        {
            Mode = MotorMode.Stand;
        }

        public void EmergencyStop()
        {
            if (isStopped) Mode = MotorMode.Stand;
            else Mode = MotorMode.EmergencyStop;
            isStopped = !isStopped;
        }

        public void MoveUp()
        {
            if (this.isStopped) return;
            Mode = MotorMode.Up;
        }

        public void MoveDown()
        {
            if (this.isStopped) return;
            Mode = MotorMode.Down;
        }
        public class MotorContext
        {
            public SPS.Pin EndTopPin;
            public SPS.Pin EndBottomPin;
        }
    }
}