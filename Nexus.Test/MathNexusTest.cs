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
        nexus.InputA = new NexusInput<double>(() => 5D);
        nexus.InputB = new NexusInput<double>(() => 10D);
        
        // result of 5 + 10 should be 15
        Assert.True((int)nexus.OutputC.Value == 15D);

        // setting the operation to multiply, the result of 5 * 10 should be 50
        nexus.Operations.GetList().SetSelected("MULTIPLY");
        Assert.True((int)nexus.OutputC.Value == 50D);
    }
}