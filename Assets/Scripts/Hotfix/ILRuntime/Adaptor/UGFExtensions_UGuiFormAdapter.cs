using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace UGFExtensions
{   
    public class UGuiFormAdapter : CrossBindingAdaptor
    {
    
        public override Type BaseCLRType
        {
            get
            {
                return typeof(UGFExtensions.UGuiForm);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : UGFExtensions.UGuiForm, CrossBindingAdaptorType,IAdapterProperty
        {
            CrossBindingMethodInfo mClose_0 = new CrossBindingMethodInfo("Close");
            CrossBindingMethodInfo<System.Object> mOnInit_1 = new CrossBindingMethodInfo<System.Object>("OnInit");
            CrossBindingMethodInfo mOnRecycle_2 = new CrossBindingMethodInfo("OnRecycle");
            CrossBindingMethodInfo<System.Object> mOnOpen_3 = new CrossBindingMethodInfo<System.Object>("OnOpen");
            CrossBindingMethodInfo<System.Boolean, System.Object> mOnClose_4 = new CrossBindingMethodInfo<System.Boolean, System.Object>("OnClose");
            CrossBindingMethodInfo mOnPause_5 = new CrossBindingMethodInfo("OnPause");
            CrossBindingMethodInfo mOnResume_6 = new CrossBindingMethodInfo("OnResume");
            CrossBindingMethodInfo mOnCover_7 = new CrossBindingMethodInfo("OnCover");
            CrossBindingMethodInfo mOnReveal_8 = new CrossBindingMethodInfo("OnReveal");
            CrossBindingMethodInfo<System.Object> mOnRefocus_9 = new CrossBindingMethodInfo<System.Object>("OnRefocus");
            CrossBindingMethodInfo<System.Single, System.Single> mOnUpdate_10 = new CrossBindingMethodInfo<System.Single, System.Single>("OnUpdate");
            CrossBindingMethodInfo<System.Int32, System.Int32> mOnDepthChanged_11 = new CrossBindingMethodInfo<System.Int32, System.Int32>("OnDepthChanged");
            CrossBindingMethodInfo<System.Boolean> mInternalSetVisible_12 = new CrossBindingMethodInfo<System.Boolean>("InternalSetVisible");
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }
            public ILRuntime.Runtime.Enviorment.AppDomain AppDomain
            {
                get => appdomain;
                set => appdomain = value;
            }

            public ILTypeInstance ILInstance 
            {
                get => instance;
                set => instance = value;
            }

            public override void Close()
            {
                if (mClose_0.CheckShouldInvokeBase(this.instance))
                    base.Close();
                else
                    mClose_0.Invoke(this.instance);
            }

            protected override void OnInit(System.Object userData)
            {
                if (mOnInit_1.CheckShouldInvokeBase(this.instance))
                    base.OnInit(userData);
                else
                    mOnInit_1.Invoke(this.instance, userData);
            }

            protected override void OnRecycle()
            {
                if (mOnRecycle_2.CheckShouldInvokeBase(this.instance))
                    base.OnRecycle();
                else
                    mOnRecycle_2.Invoke(this.instance);
            }

            protected override void OnOpen(System.Object userData)
            {
                if (mOnOpen_3.CheckShouldInvokeBase(this.instance))
                    base.OnOpen(userData);
                else
                    mOnOpen_3.Invoke(this.instance, userData);
            }

            protected override void OnClose(System.Boolean isShutdown, System.Object userData)
            {
                if (mOnClose_4.CheckShouldInvokeBase(this.instance))
                    base.OnClose(isShutdown, userData);
                else
                    mOnClose_4.Invoke(this.instance, isShutdown, userData);
            }

            protected override void OnPause()
            {
                if (mOnPause_5.CheckShouldInvokeBase(this.instance))
                    base.OnPause();
                else
                    mOnPause_5.Invoke(this.instance);
            }

            protected override void OnResume()
            {
                if (mOnResume_6.CheckShouldInvokeBase(this.instance))
                    base.OnResume();
                else
                    mOnResume_6.Invoke(this.instance);
            }

            protected override void OnCover()
            {
                if (mOnCover_7.CheckShouldInvokeBase(this.instance))
                    base.OnCover();
                else
                    mOnCover_7.Invoke(this.instance);
            }

            protected override void OnReveal()
            {
                if (mOnReveal_8.CheckShouldInvokeBase(this.instance))
                    base.OnReveal();
                else
                    mOnReveal_8.Invoke(this.instance);
            }

            protected override void OnRefocus(System.Object userData)
            {
                if (mOnRefocus_9.CheckShouldInvokeBase(this.instance))
                    base.OnRefocus(userData);
                else
                    mOnRefocus_9.Invoke(this.instance, userData);
            }

            protected override void OnUpdate(System.Single elapseSeconds, System.Single realElapseSeconds)
            {
                if (mOnUpdate_10.CheckShouldInvokeBase(this.instance))
                    base.OnUpdate(elapseSeconds, realElapseSeconds);
                else
                    mOnUpdate_10.Invoke(this.instance, elapseSeconds, realElapseSeconds);
            }

            protected override void OnDepthChanged(System.Int32 uiGroupDepth, System.Int32 depthInUIGroup)
            {
                if (mOnDepthChanged_11.CheckShouldInvokeBase(this.instance))
                    base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
                else
                    mOnDepthChanged_11.Invoke(this.instance, uiGroupDepth, depthInUIGroup);
            }

            protected override void InternalSetVisible(System.Boolean visible)
            {
                if (mInternalSetVisible_12.CheckShouldInvokeBase(this.instance))
                    base.InternalSetVisible(visible);
                else
                    mInternalSetVisible_12.Invoke(this.instance, visible);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

