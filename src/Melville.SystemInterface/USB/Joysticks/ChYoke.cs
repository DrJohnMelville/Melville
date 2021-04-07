using System;

namespace Melville.SystemInterface.USB.Joysticks
{
    public interface IChYoke
    {
        event EventHandler<EventArgs>? StateChanged;
        int Aileron { get; }
        int Elevator { get; }
        int Rudder { get; }
        int Throttle { get; }
        int Prop { get; }
        int Mixture { get; }
        
        bool LeftTrigger { get; }
        bool LeftHatUp { get; }
        bool LeftHatDown { get; }
        bool LeftHatLeft { get; }
        bool LeftHatRight { get; }
        bool LeftRockLeft { get; }
        bool LeftRockRight { get; }
        
        bool RightTrigger { get; }
        bool RightHatUp { get; }
        bool RightHatDown { get; }
        bool RightHatLeft { get; }
        bool RightHatRight { get; }
        bool RightRockLeft { get; }
        bool RightRockRight { get; }
       
        bool LeftSwitchUp { get; }
        bool LeftSwitchDown { get; }
        bool RightSwitchUp { get; }
        bool RightSwitchDown { get; }

        bool LeftLightedButton { get; }
        bool CenterButton { get; }
        bool RightLightedButton { get; }
        
        bool LeftRotery { get; }
        bool CenterRotery { get; }
        bool RightRotery { get; }
        
        bool VerticalWheelUp { get; }
        bool VerticalWheelDown { get; }
        bool VerticalWheelPress { get; }
        
        bool HorizontalWheelLeft { get; }
        bool HorizontalWheelRight { get; }
        bool HorizontalWheelPress { get; }
    }
    //aileron 1  elev 3 rudder 0B throt 5 prop 9 mix 07

    public class ChYoke : JoystickBase, IChYoke
    {
        public ChYoke(IMonitorForDeviceArrival newDeviceNodification) : 
            base("vid_068e&pid_0057", newDeviceNodification, 17)
        {
        }

        public int Aileron => Get16BitAxis(1);
        public int Elevator => Get16BitAxis(3);
        public int Rudder => Get16BitAxis(0xb);
        public int Throttle => Get16BitAxis(5);
        public int Prop => Get16BitAxis(9);
        public int Mixture => Get16BitAxis(7);
        
        // buttons
        private bool InValues(int test, int a, int b, int c) =>
            test == a || test == b || test == c;
        public bool LeftTrigger => GetButton(0x0d, 0x10);
        public bool LeftHatUp => InValues(GetAxis(0x0d), 8, 1, 2);
        public bool LeftHatDown => InValues(GetAxis(0x0d), 4, 5, 6);
        public bool LeftHatLeft =>InValues(GetAxis(0x0d), 6,7,8);
        public bool LeftHatRight => InValues(GetAxis(0x0d), 2,3,4);
        public bool LeftRockLeft => GetButton(0x0E, 0x10);
        public bool LeftRockRight => GetButton(0x0E, 0x20);
        
        public bool RightTrigger => GetButton(0x0d, 0x20);
        public bool RightHatUp => GetButton(0x0E, 0x40);
        public bool RightHatDown => GetButton(0x0F, 0x01);
        public bool RightHatLeft => GetButton(0x0F, 0x02);
        public bool RightHatRight => GetButton(0x0E, 0x80);
        public bool RightRockLeft => GetButton(0x0D, 0x40);
        public bool RightRockRight => GetButton(0x0D, 0x80);
        
        public bool LeftSwitchUp => GetButton(0x0E, 0x01);
        public bool LeftSwitchDown => GetButton(0x0E, 0x02);
        public bool RightSwitchUp => GetButton(0x0E, 0x04);
        public bool RightSwitchDown => GetButton(0x0E, 0x08);
        
        public bool LeftLightedButton => GetButton(0x10, 0x01);
        public bool CenterButton => GetButton(0x10, 0x02);
        public bool RightLightedButton => GetButton(0x10, 0x04);
        
        public bool LeftRotery => GetButton(0x10, 0x08);
        public bool CenterRotery => GetButton(0x10, 0x10);
        public bool RightRotery => GetButton(0x10, 0x20);
        
        public bool VerticalWheelUp => GetButton(0x0F, 0x04);
        public bool VerticalWheelDown => GetButton(0x0F, 0x08);
        public bool VerticalWheelPress => GetButton(0x0F, 0x10);
        
        public bool HorizontalWheelLeft => GetButton(0x0F, 0x20);
        public bool HorizontalWheelRight => GetButton(0x0F, 0x40);
        public bool HorizontalWheelPress => GetButton(0x0F, 0x80);
    }
}