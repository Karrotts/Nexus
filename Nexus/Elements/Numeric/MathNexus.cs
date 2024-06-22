using Nexus.Exceptions;

namespace Nexus.Elements.Basic;

public class MathNexus : NexusBase
{
    public readonly NexusStatic<NexusSelectableList<MathOperations>> Operations;
    public NexusInput<Double> InputA { get; set; }
    public NexusInput<Double> InputB { get; set; }
    public NexusOutput<Double> OutputC { get; set; }

    public MathNexus()
    {
        Operations = new NexusStatic<NexusSelectableList<MathOperations>>(
             new NexusSelectableList<MathOperations>(new List<MathOperations>()
             {
                 MathOperations.ADD,
                 MathOperations.SUBSTRACT,
                 MathOperations.DIVIDE,
                 MathOperations.MULTIPLY
             }, MathOperations.ADD)
        );
        
        InputA = new NexusInput<double>(() => 0);
        InputB = new NexusInput<double>(() => 0);
        OutputC = new NexusOutput<Double>(() =>
        {
            switch (Operations.Value.Selected)
            {
                case MathOperations.ADD: return InputA.Value + InputB.Value;
                case MathOperations.SUBSTRACT: return InputA.Value - InputB.Value;
                case MathOperations.MULTIPLY: return InputA.Value * InputB.Value;
                case MathOperations.DIVIDE: return InputA.Value / InputB.Value;
                default: return 0; // if this happens, then somehow the operation was null
            }
        });
    }
}