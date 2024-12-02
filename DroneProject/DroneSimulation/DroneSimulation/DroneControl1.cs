using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneSimulation
{
    class DroneControl1
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Rotation { get; set; } // Smjer rotacije

        // Funkcija za pomicanje drona u određenom smjeru
        public void Move(float direction, float speed)
        {
            // Smjer je od -1 do 1 (lijevo-desno, gore-dolje)
            X += direction * speed;
            Y += direction * speed;
            Console.WriteLine($"Drone moved to position X: {X}, Y: {Y}, Z: {Z}, Rotation: {Rotation}");
        }

        // Funkcija za rotiranje drona
        public void Rotate(float angle)
        {
            Rotation += angle;
            Console.WriteLine($"Drone rotated to angle: {Rotation}");
        }

        // Funkcija za kontroliranje visine drona
        public void ChangeAltitude(float altitude)
        {
            Z += altitude;
            Console.WriteLine($"Drone altitude changed to: {Z}");
        }
    }
}
