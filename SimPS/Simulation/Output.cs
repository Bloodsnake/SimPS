using SimPS.Components;
using System;

namespace SimPS.Simulation
{
    class Output
    {
        public static int Hight = 30;

        public static int Lenght = 60;

        public static ConsoleColor IOColor = ConsoleColor.White;

        public static ConsoleColor FalseColor = ConsoleColor.Red;

        public static ConsoleColor TrueColor = ConsoleColor.Green;

        public static void Draw(int percentage, ConsoleColor doorColor = ConsoleColor.DarkYellow)
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            DrawDoor(percentage, doorColor);
            DrawIO();
        }

        private static void DrawDoor(int doorPercent, ConsoleColor doorColor)
        {
            Console.ForegroundColor = doorColor;
            string gateInfo = "Gate: {0}%";
            for (int i = 0; i < (Lenght - (gateInfo.Length + doorPercent.ToString().Length)) / 2; i++) Console.Write("=");
            Console.Write(gateInfo, doorPercent);
            for (int i = 0; i < (Lenght - (gateInfo.Length + doorPercent.ToString().Length)) / 2; i++) Console.Write("=");
            Console.Write("\n");
            for (int i = 0; i < 20; i++)
            {
                if (doorPercent / 5 > i) for (int j = 0; j < Lenght; j++) Console.Write("#");
                else for (int j = 0; j < Lenght; j++) Console.Write(" ");
                Console.Write("\n");
            }
            GC.Collect();
        }
        private static void DrawIO()
        {
            foreach (var pin in SPS.Pin.KeyMap)
            {
                Console.ForegroundColor = IOColor;
                Console.Write(" {0} ({1}): ", pin.Value.Id, pin.Value.MappedKey);
                Console.ForegroundColor = pin.Value.Value ? TrueColor : FalseColor;
                Console.Write(pin.Value.Value);
            }
            for (int i = 0; i < 20; i++) Console.Write(" ");
            Console.Write("\n");
        }
    }
}
