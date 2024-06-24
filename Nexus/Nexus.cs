using System.Reflection;
using Nexus.Exceptions;

namespace Nexus;

public static class Nexus
{
    
    [Obsolete("Use ConnectIO instead..")]
    public static void ConnectOutputToInput(INexus outputNexus, INexus inputNexus, string outputName, string inputName)
    {
        Type inputType = getInput(inputNexus, inputName);
        Type outputType = getOutput(outputNexus, outputName);
        
        if (inputType != outputType)
            throw new NexusIOTypeMismatchException("Output expected type "+ outputType.Name +", received type " + inputType.Name);
        
        Type nodeInputType = typeof(NexusInput<>).MakeGenericType(inputType);
        Type nodeInputDelegate = typeof(INexusIO.GetterDelegate).MakeGenericType(inputType);
        ConstructorInfo constructorInfo = nodeInputType.GetConstructor(new[] { nodeInputDelegate });
        inputNexus.SetInstanceProperty(inputName, constructorInfo.Invoke(new object[]
        {
            outputNexus.GetPropValue(outputName).GetType().GetProperty("Getter").GetValue(outputNexus.GetPropValue(outputName))
        }));
    }

    public static void ConnectIO(INexus outputNexus, INexus inputNexus, string outputName, string inputName)
    {
        INexusIO outputIo = GetIO(outputNexus, outputName);
        INexusIO inputIo = GetIO(inputNexus, inputName);
        inputIo.SetGetter(() => outputIo.CastToType(inputIo.GetGenericType()));
    }
    
    /**
     * Set the io to a default value of 0D since that can be cast into
     * all the different types safely
     */
    public static void DisconnectIO(INexus nexus, string ioName)
    {
        GetIO(nexus, ioName).SetGetter(() => 0D);
    }

    public static INexusIO GetIO(INexus nexus, string ioName)
    {
        PropertyInfo? prop = nexus.GetType().GetProperty(ioName);
        if (prop == null)
            throw new Exception($"IO with name {ioName} was not found in nexus");
        return (INexusIO)prop.GetValue(nexus);
    }

    private static Type getInput(INexus input, string inputName)
    {
        Dictionary<string, Type> inputs = input.GetInputs();
        if (inputs.ContainsKey(inputName)) return inputs[inputName];
        throw new NexusIOValueDoesNotExistException("Node does not have an input property with name: " + inputName);
    }

    private static Type getOutput(INexus output, string outputName)
    {
        Dictionary<string, Type> outputs = output.GetOutputs();
        if (outputs.ContainsKey(outputName)) return outputs[outputName];
        throw new NexusIOValueDoesNotExistException("Node does not have an output property with name: " + outputName);
    }
}