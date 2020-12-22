using System;

namespace Melville.MVVM.USB.Joysticks
{
    public interface IJoystick
    {
        public byte XAxis { get; }
        public byte YAxis { get; }
        public byte ZAxis { get; }
        public byte Throttle { get; }
        public bool Button1 { get; }
        public bool Button2 { get; }
        public bool Button3 { get; }
        public bool Button4 { get; }
        public bool Button5 { get; }
        public bool Button6 { get; }
        public bool Button7 { get; }
        public bool Button8 { get; }
        public bool Button9 { get; }
        public bool Button10 { get; }
        public bool Button11 { get; }
        public bool Button12 { get; }
        public bool HatLeft { get; }
        public bool HatRight { get; }
        public bool HatUp { get; }
        public bool HatDown { get; }
        event EventHandler<EventArgs>? StateChanged;
    }

    public class Joystick: JoystickBase, IJoystick
    {
        public Joystick(IMonitorForDeviceArrival newDeviceNodification) :
            base("vid_044f&pid_b106", newDeviceNodification, 16)
        {
        }

        public byte XAxis => GetAxis(4);
        public byte YAxis => GetAxis(5);
        public byte ZAxis => GetAxis(6);
        public byte Throttle => GetAxis(7);
        public bool Button1  => GetButton(1, 0x01);
        public bool Button2  => GetButton(1, 0x02);
        public bool Button3  => GetButton(1, 0x04);
        public bool Button4  => GetButton(1, 0x08);
        public bool Button5  => GetButton(1, 0x10);
        public bool Button6  => GetButton(1, 0x20);
        public bool Button7  => GetButton(1, 0x40);
        public bool Button8  => GetButton(1, 0x80);
        public bool Button9  => GetButton(2, 0x01);
        public bool Button10 => GetButton(2, 0x02);
        public bool Button11 => GetButton(2, 0x04);
        public bool Button12 => GetButton(2, 0x08);
        public bool HatLeft  => GetButton(9, 0xFF);
        public bool HatRight => GetButton(8, 0xFF);
        public bool HatUp    => GetButton(10, 0xFF);
        public bool HatDown  => GetButton(11, 0xFF);
    }
}