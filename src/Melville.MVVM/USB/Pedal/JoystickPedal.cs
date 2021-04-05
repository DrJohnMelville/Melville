using System;
using System.ComponentModel;
using Melville.MVVM.USB.Joysticks;

namespace Melville.MVVM.USB.Pedal
{
    public class JoystickPedal: ITranscriptonPedal
    {
        public JoystickPedal(IJoystick joystick)
        {
            left = new ButtonMonitor(()=>joystick.Button8, "Left", OnPropertyChange, ()=>LeftUp, ()=>LeftDown);
            center = new ButtonMonitor(()=>joystick.Button7||joystick.Button9, "Center", OnPropertyChange, 
                ()=>CenterUp, ()=>CenterDown);
            right = new ButtonMonitor(()=>joystick.Button10, "Right", OnPropertyChange, ()=>RightUp, ()=>RightDown);

            joystick.StateChanged += left.StateChanged;
            joystick.StateChanged += center.StateChanged;
            joystick.StateChanged += right.StateChanged;
        }

        private void OnPropertyChange(string property) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private ButtonMonitor left;
        private ButtonMonitor center;
        private ButtonMonitor right;
        public bool Left => left.Value;
        public bool Center => center.Value;
        public bool Right => right.Value;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? LeftDown;
        public event EventHandler? LeftUp;
        public event EventHandler? CenterDown;
        public event EventHandler? CenterUp;
        public event EventHandler? RightDown;
        public event EventHandler? RightUp;

        private class ButtonMonitor
        {
            public bool Value { get; private set; }
            private readonly Func<bool> source;
            private string name;
            private Action<string> parentChange;
            private Func<EventHandler?> UpEvent;
            private Func<EventHandler?> DownEvent;

            public ButtonMonitor(Func<bool> source, string name, Action<string> parentChange, 
                Func<EventHandler?> upEvent, Func<EventHandler?> downEvent)
            {
                this.source = source;
                this.name = name;
                this.parentChange = parentChange;
                UpEvent = upEvent;
                DownEvent = downEvent;
            }

            public void StateChanged(object? stick, EventArgs ea)
            {
                var newVaue = source();
                if (Value == newVaue) return;
                Value = newVaue;
                parentChange(name);
                (Value?DownEvent:UpEvent)()?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}