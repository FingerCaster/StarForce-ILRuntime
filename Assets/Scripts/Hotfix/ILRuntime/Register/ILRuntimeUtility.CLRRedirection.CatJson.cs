using ILRuntime.Runtime.Enviorment;

namespace UGFExtensions.Hotfix
{
    public partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Redirection)]
        public static void RegisterCatJsonCLRRedirection(AppDomain appDomain)
        {
            CatJson.JsonParser.RegisterILRuntimeCLRRedirection(appDomain);
        }
    }
}