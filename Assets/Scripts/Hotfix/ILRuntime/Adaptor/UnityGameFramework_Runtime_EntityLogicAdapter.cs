using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace UGFExtensions
{   
    public class EntityLogicAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(UnityGameFramework.Runtime.EntityLogic);
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

        public class Adapter : UnityGameFramework.Runtime.EntityLogic, CrossBindingAdaptorType,IAdapterProperty
        {
            CrossBindingMethodInfo<System.Object> mOnInit_0 = new CrossBindingMethodInfo<System.Object>("OnInit");
            CrossBindingMethodInfo mOnRecycle_1 = new CrossBindingMethodInfo("OnRecycle");
            CrossBindingMethodInfo<System.Object> mOnShow_2 = new CrossBindingMethodInfo<System.Object>("OnShow");
            CrossBindingMethodInfo<System.Boolean, System.Object> mOnHide_3 = new CrossBindingMethodInfo<System.Boolean, System.Object>("OnHide");
            CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object> mOnAttached_4 = new CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>("OnAttached");
            CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, System.Object> mOnDetached_5 = new CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, System.Object>("OnDetached");
            CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object> mOnAttachTo_6 = new CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, UnityEngine.Transform, System.Object>("OnAttachTo");
            CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, System.Object> mOnDetachFrom_7 = new CrossBindingMethodInfo<UnityGameFramework.Runtime.EntityLogic, System.Object>("OnDetachFrom");
            CrossBindingMethodInfo<System.Single, System.Single> mOnUpdate_8 = new CrossBindingMethodInfo<System.Single, System.Single>("OnUpdate");
            CrossBindingMethodInfo<System.Boolean> mInternalSetVisible_9 = new CrossBindingMethodInfo<System.Boolean>("InternalSetVisible");
            CrossBindingMethodInfo<UnityEngine.Collider> mOnOnTriggerEnter_1 = new CrossBindingMethodInfo<UnityEngine.Collider>("OnTriggerEnter");

            bool isInvokingToString;
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

            public ILTypeInstance ILInstance { get { return instance; } }

            ILRuntime.Runtime.Enviorment.AppDomain IAdapterProperty.AppDomain
            {
                get => appdomain;
                set => appdomain = value;
            }

            ILTypeInstance IAdapterProperty.ILInstance 
            {
                get => instance;
                set => instance = value;
            }

            protected override void OnInit(System.Object userData)
            {
                if (mOnInit_0.CheckShouldInvokeBase(this.instance))
                    base.OnInit(userData);
                else
                    mOnInit_0.Invoke(this.instance, userData);
            }

            protected override void OnRecycle()
            {
                if (mOnRecycle_1.CheckShouldInvokeBase(this.instance))
                    base.OnRecycle();
                else
                    mOnRecycle_1.Invoke(this.instance);
            }

            protected override void OnShow(System.Object userData)
            {
                if (mOnShow_2.CheckShouldInvokeBase(this.instance))
                    base.OnShow(userData);
                else
                    mOnShow_2.Invoke(this.instance, userData);
            }

            protected override void OnHide(System.Boolean isShutdown, System.Object userData)
            {
                if (mOnHide_3.CheckShouldInvokeBase(this.instance))
                    base.OnHide(isShutdown, userData);
                else
                    mOnHide_3.Invoke(this.instance, isShutdown, userData);
            }

            protected override void OnAttached(UnityGameFramework.Runtime.EntityLogic childEntity, UnityEngine.Transform parentTransform, System.Object userData)
            {
                if (mOnAttached_4.CheckShouldInvokeBase(this.instance))
                    base.OnAttached(childEntity, parentTransform, userData);
                else
                    mOnAttached_4.Invoke(this.instance, childEntity, parentTransform, userData);
            }

            protected override void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, System.Object userData)
            {
                if (mOnDetached_5.CheckShouldInvokeBase(this.instance))
                    base.OnDetached(childEntity, userData);
                else
                    mOnDetached_5.Invoke(this.instance, childEntity, userData);
            }

            protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity, UnityEngine.Transform parentTransform, System.Object userData)
            {
                if (mOnAttachTo_6.CheckShouldInvokeBase(this.instance))
                    base.OnAttachTo(parentEntity, parentTransform, userData);
                else
                    mOnAttachTo_6.Invoke(this.instance, parentEntity, parentTransform, userData);
            }

            protected override void OnDetachFrom(UnityGameFramework.Runtime.EntityLogic parentEntity, System.Object userData)
            {
                if (mOnDetachFrom_7.CheckShouldInvokeBase(this.instance))
                    base.OnDetachFrom(parentEntity, userData);
                else
                    mOnDetachFrom_7.Invoke(this.instance, parentEntity, userData);
            }

            protected override void OnUpdate(System.Single elapseSeconds, System.Single realElapseSeconds)
            {
                if (mOnUpdate_8.CheckShouldInvokeBase(this.instance))
                    base.OnUpdate(elapseSeconds, realElapseSeconds);
                else
                    mOnUpdate_8.Invoke(this.instance, elapseSeconds, realElapseSeconds);
            }

            protected override void InternalSetVisible(System.Boolean visible)
            {
                if (mInternalSetVisible_9.CheckShouldInvokeBase(this.instance))
                    base.InternalSetVisible(visible);
                else
                    mInternalSetVisible_9.Invoke(this.instance, visible);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
            private void OnTriggerEnter(UnityEngine.Collider collider)
            {
                mOnOnTriggerEnter_1.Invoke(this.instance, collider);
            }
        }
    }
}

