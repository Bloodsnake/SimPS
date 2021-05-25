using SimPS.Components;
using SimPS.Simulation;
using System.Timers;

namespace SimPS
{
    class Program
    {
        static void Main(string[] args)
        {
            var sps = new SPS();
            var mx = new Motor.MotorContext()
            {
                EndTopPin = sps.Pins.E0,
                EndBottomPin = sps.Pins.E1
            };

            var motor = new Motor(mx);

            //sps.Pins.A0.MappedKey = System.Windows.Forms.Keys.U;
            //sps.Pins.A1.MappedKey = System.Windows.Forms.Keys.D;
            sps.Pins.E0.MappedKey = System.Windows.Forms.Keys.D0;
            sps.Pins.E1.MappedKey = System.Windows.Forms.Keys.D1;
            sps.Pins.E2.MappedKey = System.Windows.Forms.Keys.D2;
            sps.Pins.E3.MappedKey = System.Windows.Forms.Keys.D3;
            sps.Pins.E4.MappedKey = System.Windows.Forms.Keys.S;
            sps.Pins.E5.MappedKey = System.Windows.Forms.Keys.L;







            bool LightSensorUsed = false;

            var timer = new Timer();
            timer.Interval = 10000;
            timer.Elapsed += (o, a) =>
            {
                motor.MoveDown();
                timer.Stop();
            };

            SPS.Pin.PinChanged += (o, eventSPS) =>
            {
                if (eventSPS.Pin.Id == sps.Pins.E5.Id) 
                { 
                    motor.MoveUp();
                    //Timer
                    LightSensorUsed = true;
                }
                //Notschalter
                if (eventSPS.Pin.Id == sps.Pins.E4.Id)
                {
                    motor.EmergencyStop();
                }
                //Auf
                if (eventSPS.Pin.Id == sps.Pins.E2.Id)
                {
                    motor.MoveUp();
                    timer.Stop();
                }
                //Zu
                if (eventSPS.Pin.Id == sps.Pins.E3.Id)
                {
                    motor.MoveDown();
                }
                if (motor.Percentage >= 99 && LightSensorUsed) timer.Start();
            };
            Inputs.StartGlobal();

        }
    }
}
