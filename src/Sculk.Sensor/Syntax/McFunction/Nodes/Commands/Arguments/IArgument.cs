namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments; 

public interface IArgument<out TValue> {
    TValue Value { get; }
    
    static abstract IArgument<TValue> Parse(string[] args, ref int index);
}
