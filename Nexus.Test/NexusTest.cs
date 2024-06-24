using Nexus.Elements.Basic;
using Nexus.Elements.Display;

namespace Nexus.Test;

public class NexusTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CanConnectTwoLikeIO()
    {
        MathNexus mathNexus = new MathNexus()
        {
            InputA = new NexusInput<double>(() => 15D),
            InputB = new NexusInput<double>(() => 15D)
        };
        
        MathNexus math2Nexus = new MathNexus()
        {
            InputA = new NexusInput<double>(() => 20D),
        };
        
        Nexus.ConnectIO(mathNexus, math2Nexus, "OutputC", "InputB");
        Assert.True(math2Nexus.OutputC.Value == 50);
    }

    [Test]
    public void CanConnectTwoUnlinkIO()
    {
        MathNexus mathNexus = new MathNexus()
        {
            InputA = new NexusInput<double>(() => 15D),
            InputB = new NexusInput<double>(() => 15D)
        };

        OutputNexus outputNexus = new OutputNexus();
        
        Nexus.ConnectIO(mathNexus, outputNexus, "OutputC", "DisplayInput");
        Assert.True(outputNexus.DisplayInput.Value == "30");
    }
    
    [Test]
    public void TestCastOperations()
    {
        int a = 5;
        double b = 10.4D;
        bool c = true;
        string d = "true";

        double a_a = (double)Convert.ChangeType(a, typeof(double));
        int b_a = (int)Convert.ChangeType(b, typeof(int));
        double c_a = (double)Convert.ChangeType(c, typeof(double));

        double f = a_a * b_a;
    }
}