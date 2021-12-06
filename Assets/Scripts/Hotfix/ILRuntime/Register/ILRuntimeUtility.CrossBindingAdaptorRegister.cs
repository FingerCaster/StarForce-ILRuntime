using ILRuntime.Runtime.Enviorment;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Adaptor)]
        public static void RegisterCrossBindingAdaptor(AppDomain appDomain)
        {
            //注册跨域继承适配器
            // appDomain.RegisterCrossBindingAdaptor(new IDisposableAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
            appDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
            appDomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new ObjectBaseAdapter());
            appDomain.RegisterCrossBindingAdaptor(new UGuiFormAdapter());
            appDomain.RegisterCrossBindingAdaptor(new EntityLogicAdapter());
        }
    }
}