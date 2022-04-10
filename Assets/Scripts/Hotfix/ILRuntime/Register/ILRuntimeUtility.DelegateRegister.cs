using System;
using ILRuntime.Runtime.Intepreter;
using UnityEngine.Events;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Delegate)]
        public static void RegisterDelegate(AppDomain appDomain)
        {
            appDomain.DelegateManager.RegisterMethodDelegate<float>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, ILTypeInstance>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, GameFramework.Event.GameEventArgs>();
            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.U2D.SpriteAtlas>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.String, System.Object, System.Single, System.Object>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.String>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.String, GameFramework.Resource.LoadResourceStatus, System.String, System.Object>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Object>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int64, UGFExtensions.LoopTask>();

            //注册委托
            appDomain.DelegateManager.RegisterDelegateConvertor<GameFramework.GameFrameworkAction<System.Object>>(
                (act) =>
                {
                    return new GameFramework.GameFrameworkAction<System.Object>((obj) =>
                    {
                        ((Action<System.Object>)act)(obj);
                    });
                });
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>)act)(arg0);
                });
            });


            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) =>
            {
                return new UnityAction(() => { ((Action)action)(); });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((action) =>
            {
                return new UnityAction<float>((a) => { ((Action<float>)action)(a); });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<EventHandler<GameFramework.Event.GameEventArgs>>(
                (act) =>
                {
                    return new EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                    {
                        ((Action<object, GameFramework.Event.GameEventArgs>)act)(sender, e);
                    });
                });

            appDomain.DelegateManager.RegisterDelegateConvertor<EventHandler<ILTypeInstance>>((act) =>
            {
                return new EventHandler<ILTypeInstance>((sender, e) =>
                {
                    ((Action<object, ILTypeInstance>)act)(sender, e);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetSuccessCallback>(
                (act) =>
                {
                    return new GameFramework.Resource.LoadAssetSuccessCallback((assetName, asset, duration, userData) =>
                    {
                        ((Action<System.String, System.Object, System.Single, System.Object>)act)(assetName, asset,
                            duration, userData);
                    });
                });
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>)act)(arg0);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetFailureCallback>(
                (act) =>
                {
                    return new GameFramework.Resource.LoadAssetFailureCallback(
                        (assetName, status, errorMessage, userData) =>
                        {
                            ((Action<System.String, GameFramework.Resource.LoadResourceStatus, System.String,
                                System.Object>)act)(assetName, status, errorMessage, userData);
                        });
                });
        }
    }
}