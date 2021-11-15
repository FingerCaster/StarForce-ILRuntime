using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;


public interface IAdapterProperty
{
    AppDomain AppDomain { get; set; }
    ILTypeInstance ILInstance { get; set; }
}