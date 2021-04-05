using System;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public abstract class GraphValue
  {
    public abstract double FinalValue(Axis axis);
    public double FinalFraction(Axis axis) => FinalValue(axis) / axis.TargetRange;
    public static GraphValue operator +(GraphValue left, GraphValue right) => new GraphBinaryExpression(left, GraphValueOperation.Add, right);
    public static GraphValue operator -(GraphValue left, GraphValue right) => new GraphBinaryExpression(left, GraphValueOperation.Subtract, right);
    public static GraphValue operator *(GraphValue left, GraphValue right) => new GraphBinaryExpression(left, GraphValueOperation.Multiply, right);
    public static GraphValue operator /(GraphValue left, GraphValue right) => new GraphBinaryExpression(left, GraphValueOperation.Divide, right);
    public static GraphValue operator +(GraphValue left, double right) =>
      new GraphBinaryExpression(left, GraphValueOperation.Add, new PrecomputedValue(right));
    public static GraphValue operator -(GraphValue left, double right) =>
      new GraphBinaryExpression(left, GraphValueOperation.Subtract, new PrecomputedValue(right));
    public static GraphValue operator *(GraphValue left, double right) =>
      new GraphBinaryExpression(left, GraphValueOperation.Multiply, new PrecomputedValue(right));
    public static GraphValue operator /(GraphValue left, double right) =>
      new GraphBinaryExpression(left, GraphValueOperation.Divide, new PrecomputedValue(right));
  }

  public abstract class GraphValueWithValue: GraphValue
  {
    protected GraphValueWithValue(double rawValue)
    {
      RawValue = rawValue;
    }

    public double RawValue { get; }
  }

  public enum GraphValueOperation { Add = 0, Subtract = 1, Multiply = 3, Divide = 4}

  public class GraphBinaryExpression : GraphValue
  {
    private readonly GraphValue left;
    private readonly GraphValueOperation operation;
    private readonly GraphValue right;

    public GraphBinaryExpression(GraphValue left, GraphValueOperation operation, GraphValue right)
    {
      this.left = left;
      this.operation = operation;
      this.right = right;
    }

    public override double FinalValue(Axis axis)
    {
      var leftVal = left.FinalValue(axis);
      var rightVal = right.FinalValue(axis);
      switch (operation)
      {
        case GraphValueOperation.Add: return leftVal + rightVal;
        case GraphValueOperation.Subtract: return leftVal - rightVal;
        case GraphValueOperation.Multiply: return leftVal * rightVal;
        case GraphValueOperation.Divide:
          return leftVal / rightVal;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  } 

  public class ScaledValue : GraphValueWithValue
  {
    public ScaledValue(double rawValue) : base(rawValue)
    {
    }

    public override double FinalValue(Axis axis) => axis.ScaledValue(RawValue);
  }

  public class RelativeValue : GraphValueWithValue
  {
    public RelativeValue(double rawValue) : base(rawValue)
    {
    }

    public override double FinalValue(Axis axis) => axis.Relative(RawValue);
  }

  public class PrecomputedValue: GraphValueWithValue
  {
    public PrecomputedValue(double rawValue) : base(rawValue)
    {
    }

    public override double FinalValue(Axis axis) => RawValue;
  }
}