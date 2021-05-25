using Gma.System.MouseKeyHook;
using SimPS.Components;
using System.Windows.Forms;

namespace SimPS.Simulation
{
    class Inputs
    {
        public static void StartGlobal()
        {
            Hook.GlobalEvents().KeyDown += (sender, e) =>
            {
                foreach (var pin in SPS.Pin.KeyMap)
                {
                    if (pin.Key == e.KeyCode)
                    {
                        pin.Value.Value = !pin.Value.StandartPosition;
                        return;
                    }
                }
            };
            Hook.GlobalEvents().KeyUp += (sender, e) =>
            {
                foreach (var pin in SPS.Pin.KeyMap)
                {
                    if (pin.Key == e.KeyCode)
                    {
                        pin.Value.Value = pin.Value.StandartPosition;
                        return;
                    }
                }
            };
            Application.Run(new ApplicationContext());
        }
    }
}
