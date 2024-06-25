namespace Nexus.Elements.Boolean;

public class LogicNexus : NexusBase
{
    public NexusStatic LogicOperation;
    public NexusOutput<bool> Result { get; set; }
    public NexusInput<bool> InputA { get; set; }
    public NexusInput<bool> InputB { get; set; }
    
    public LogicNexus()
    {
        LogicOperation = new NexusStatic(new NexusSelectableList<LogicOperations>(new List<LogicOperations>()
        {
            LogicOperations.AND,
            LogicOperations.NAND,
            LogicOperations.OR,
            LogicOperations.XOR,
            LogicOperations.NOR,
            LogicOperations.XNOR,
        }, LogicOperations.AND));
        InputA = new NexusInput<bool>(() => false);
        InputB = new NexusInput<bool>(() => false);
        Result = new NexusOutput<bool>(() => DetermineResult());
    }

    public bool DetermineResult()
    {
        LogicOperations operation = (LogicOperation.Value as NexusSelectableList<LogicOperations>).Selected;
        switch (operation)
        {
            case LogicOperations.AND:
                return InputA.Value && InputB.Value;
            case LogicOperations.NAND:
                return !(InputA.Value && InputB.Value);
            case LogicOperations.OR:
                return InputA.Value || InputB.Value;
            case LogicOperations.NOR:
                return !(InputA.Value || InputB.Value);
            case LogicOperations.XOR:
                return InputA.Value ^ InputB.Value;
            case LogicOperations.XNOR:
                return !(InputA.Value ^ InputB.Value);
        }
        return false;
    }
}