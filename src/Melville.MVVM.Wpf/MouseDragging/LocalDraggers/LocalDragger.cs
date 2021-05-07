using System;
using System.ComponentModel.Design.Serialization;
using System.Windows;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Input;
using Melville.Hacks;
using Melville.MVVM.Undo;
using Melville.MVVM.Wpf.EventBindings;
using Melville.MVVM.Wpf.KeyboardFacade;

namespace Melville.MVVM.Wpf.MouseDragging.LocalDraggers
{
    public interface ILocalDragger<T> where T: struct
    {
        void NewPoint(MouseMessageType type, T point);
    }

    public static class LocalDragger
    {
        public static ILocalDragger<Point> Action(Action<MouseMessageType, Point> action) =>
            new LambdaDragger<Point>(action);
        public static ILocalDragger<Point> Action(Action<Point> action) =>
            Action((_, point)=>action(point));
        public static ILocalDragger<CircularPoint> CircleAction(Action<MouseMessageType, CircularPoint> action) =>
            new LambdaDragger<CircularPoint>(action);
        public static ILocalDragger<CircularPoint> CircleAction(Action<CircularPoint> action) =>
            CircleAction((_, point)=>action(point));

        public static ILocalDragger<T> Undo<T>(UndoEngine undo, ILocalDragger<T> effector)
            where T: struct =>
            new UndoDragger<T>(undo, effector);

        public static ILocalDragger<Point> MinimumDrag(ILocalDragger<Point> effector) =>
            MinimumDrag(SystemParameters.MinimumVerticalDragDistance, effector);
        public static ILocalDragger<Point> MinimumDrag(double radius, ILocalDragger<Point> effector) =>
            new MinimumDragger(radius, effector);

        public static ILocalDragger<Point> RectToCircle(
            Point origin, ILocalDragger<CircularPoint> target) =>
            RectToCircle(origin, new Vector(1, 0), target);
        public static ILocalDragger<Point> RectToCircle(
            Point origin, Vector zero, ILocalDragger<CircularPoint> target) =>
            Action((type, pt) => target.NewPoint(type, CircularPoint.FromVectors(zero, pt - origin)));

        public static ILocalDragger<CircularPoint> SnapToAngle(
            int snapPoints, double width, ILocalDragger<CircularPoint> target) =>
            new CircleSnapper(snapPoints, width, target);

        public static ILocalDragger<CircularPoint> SnapMouseUpToAngle(
            int snapPoints, double width, ILocalDragger<CircularPoint> target) =>
            new MouseUpCircleSnapper(snapPoints, width, target);

        public static ILocalDragger<Point> Delta(ILocalDragger<Point> target) =>
            new DeltaDragger(target);

        public static ILocalDragger<Point> InitialPoint(
            double iX, double iY, ILocalDragger<Point> target) => InitialPoint(new Point(iX, iY), target);

        public static ILocalDragger<Point> InitialPoint(Point origin, ILocalDragger<Point> target) =>
            new InitialPointDragger(origin, target);

        public static ILocalDragger<Point> GridSnapping(double snapSize, ILocalDragger<Point> target) =>
            Action((type, point) => target.NewPoint(type,
                new Point(SnapToGrid(snapSize, point.X), SnapToGrid(snapSize, point.Y))));

        private static double SnapToGrid(double snapSize, double dimension) => 
            snapSize * Math.Round(dimension / snapSize);

        public static ILocalDragger<T> OnKey<T>(
            ModifierKeys modifier, Func<ILocalDragger<T>, ILocalDragger<T>> whenPressed,
            ILocalDragger<T> unpressed) where T : struct =>
            OnKey<T>(new KeyboardQuery(), modifier, whenPressed, unpressed);

        public static ILocalDragger<T> OnKey<T>(IKeyboardQuery keyboard,
            ModifierKeys modifier, Func<ILocalDragger<T>, ILocalDragger<T>> whenPressed,
            ILocalDragger<T> unpressed) where T : struct =>
            new KeySwitcherDragger<T>(keyboard, modifier, whenPressed(unpressed), unpressed);

        public static ILocalDragger<Point> FieldDragger(
            double initialX, double initialY, ILocalDragger<Point> target) =>
            FieldDragger(new KeyboardQuery(), new Point(initialX, initialY), target);
        
        private static ILocalDragger<Point> FieldDragger(Point initialPoint, ILocalDragger<Point> target) =>
            FieldDragger(new KeyboardQuery(), initialPoint, target);
        
        public static ILocalDragger<Point> FieldDragger(IKeyboardQuery keyboard,
            double initialX, double initialY, ILocalDragger<Point> target) =>
            FieldDragger(keyboard, new Point(initialX, initialY), target);
        
        private static ILocalDragger<Point> FieldDragger(
            IKeyboardQuery keyboard, Point initialPoint, ILocalDragger<Point> target) =>
            InitialPoint(initialPoint,
                OnKey(keyboard, ModifierKeys.Shift, i => new RestrictToAxis(i),
                    OnKey(keyboard, ModifierKeys.Alt, i => GridSnapping(5, i), target)));

        public static ILocalDragger<Point> Constrain(
            double minX, double maxX, int minY, double maxY, ILocalDragger<Point> target) =>
            Constrain(new Rect(new Point(minX, minY), new Point(maxX, maxY)), target);

        private static ILocalDragger<Point> Constrain(Rect bounds, ILocalDragger<Point> target) =>
            Action((type, pt) => target.NewPoint(type, bounds.ApplyConstraint(pt)));
        public static Point ApplyConstraint(this Rect constraint, Point point) =>
            new Point(point.X.Clamp(constraint.Left, constraint.Right),
                point.Y.Clamp(constraint.Top, constraint.Bottom));

        public static ILocalDragger<Point> InvertY(double max, ILocalDragger<Point> target) => 
            Action((type, point) => target.NewPoint(type, new Point(point.X, max - point.Y))); 
        
        public static ILocalDragger<Point> ScaleDragger(
            double scaleX, double scaleY, ILocalDragger<Point> target) => 
            Action((type, point) => target.NewPoint(type, new Point(point.X*scaleX, point.Y*scaleY)));

        public static ILocalDragger<T> MaxMoves<T>(int count, ILocalDragger<T> target) where T:struct
        {
            return new LambdaDragger<T>((type, pt) =>
            {
                if (count-- > 0) target.NewPoint(type, pt);
            });
        }
    }
    
}