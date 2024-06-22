using Nexus.Elements.Basic;

namespace Nexus.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void MathNexusCanDoOperations()
    {
        MathNexus nexus = new MathNexus();
        nexus.InputA = new NexusInput<double>(() => 5);
        nexus.InputB = new NexusInput<double>(() => 10);
        
        // result of 5 + 10 should be 15
        Assert.True((int)nexus.OutputC.Value == 15);

        // setting the operation to multiply, the result of 5 * 10 should be 50
        nexus.Operations.GetList().SetSelected("MULTIPLY");
        Assert.True((int)nexus.OutputC.Value == 50);
    }
}