using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }

        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3> s_UnityEngine_Vector3_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion> s_UnityEngine_Quaternion_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2> s_UnityEngine_Vector2_Binding_Binder = null;

        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UGFExtensions_GameEntry_Binding.Register(app);
            UnityGameFramework_Runtime_ShowEntitySuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_EventComponent_Binding.Register(app);
            UnityGameFramework_Runtime_ShowEntityFailureEventArgs_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UGFExtensions_EntityExtensions_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            System_Type_Binding.Register(app);
            UnityGameFramework_Runtime_Entity_Binding.Register(app);
            UnityGameFramework_Runtime_Log_Binding.Register(app);
            UGFExtensions_DataTableExtensionComponent_Binding.Register(app);
            UnityEngine_Collider_Binding.Register(app);
            UnityEngine_Bounds_Binding.Register(app);
            GameFramework_Utility_Binding_Random_Binding.Register(app);
            System_Threading_Interlocked_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            UGFExtensions_Hotfix_HotfixComponent_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding_Enumerator_Binding.Register(app);
            System_Activator_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Action_2_Single_Single_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Action_1_Boolean_Binding.Register(app);
            System_Action_Binding.Register(app);
            System_Char_Binding.Register(app);
            UGFExtensions_LoadHotfixDataTableUserData_Binding.Register(app);
            GameFramework_DataTable_DataTableBase_Binding.Register(app);
            UGFExtensions_Await_AwaitableExtension_Binding.Register(app);
            GameFramework_DataTable_IDataTable_1_DRHotfix_Binding.Register(app);
            UGFExtensions_DRHotfix_Binding.Register(app);
            UnityGameFramework_Runtime_DataTableComponent_Binding.Register(app);
            UGFExtensions_UIExtension_Binding.Register(app);
            ILRuntime_Reflection_ILRuntimeType_Binding.Register(app);
            ILRuntime_CLR_TypeSystem_ILType_Binding.Register(app);
            UnityEngine_LayerMask_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            UnityGameFramework_Runtime_EntityLogic_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            System_Int32_Binding.Register(app);
            GameFramework_Utility_Binding_Text_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            UGFExtensions_DRAircraft_Binding.Register(app);
            UGFExtensions_DRArmor_Binding.Register(app);
            UGFExtensions_DRAsteroid_Binding.Register(app);
            UGFExtensions_DRThruster_Binding.Register(app);
            UGFExtensions_DRWeapon_Binding.Register(app);
            UnityGameFramework_Runtime_EntityComponent_Binding.Register(app);
            UGFExtensions_DREntity_Binding.Register(app);
            UGFExtensions_AssetUtility_Binding.Register(app);
            System_Exception_Binding.Register(app);
            System_Threading_Tasks_Task_Binding.Register(app);
            System_Collections_Generic_List_1_UGFExtensions_EntityLogicAdapter_Binding_Adapter_Binding.Register(app);
            UGFExtensions_SoundExtension_Binding.Register(app);
            UnityEngine_Random_Binding.Register(app);
            UnityEngine_Rect_Binding.Register(app);
            UnityEngine_Input_Binding.Register(app);
            UnityEngine_Camera_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            UnityExtension_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            System_EventHandler_1_ILTypeInstance_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            GameFramework_GameFrameworkException_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Variable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncVoidMethodBuilder_Binding.Register(app);
            UGFExtensions_CancellationToken_Binding.Register(app);
            GameFramework_ObjectPool_IObjectPool_1_UGFExtensions_ObjectBaseAdapter_Binding_Adapter_Binding.Register(app);
            GameFramework_ObjectPool_ObjectBase_Binding.Register(app);
            UnityEngine_Canvas_Binding.Register(app);
            UnityEngine_UI_CanvasScaler_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            UnityGameFramework_Runtime_ObjectPoolComponent_Binding.Register(app);
            System_Threading_Tasks_Task_1_GameObject_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_GameObject_Binding.Register(app);
            UnityEngine_CanvasGroup_Binding.Register(app);
            UnityEngine_UI_Slider_Binding.Register(app);
            UnityGameFramework_Runtime_SceneComponent_Binding.Register(app);
            UnityEngine_RectTransformUtility_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_Boolean_Binding.Register(app);
            System_Threading_Tasks_Task_1_Boolean_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Boolean_Binding.Register(app);
            System_Math_Binding.Register(app);
            UGFExtensions_Timer_TimerComponent_Binding.Register(app);
            GameFramework_ReferencePool_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneSuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneFailureEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneUpdateEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadSceneDependencyAssetEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_SoundComponent_Binding.Register(app);
            UnityGameFramework_Runtime_BaseComponent_Binding.Register(app);
            UnityGameFramework_Runtime_VarInt32_Binding.Register(app);
            UGFExtensions_DRScene_Binding.Register(app);
            System_Single_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            UnityGameFramework_Runtime_VarObject_Binding.Register(app);
            GameFramework_Variable_1_Object_Binding.Register(app);
            UnityGameFramework_Runtime_LoadDataTableSuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadDataTableFailureEventArgs_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Byte_ILTypeInstance_Binding.Register(app);
            GameFramework_Variable_1_Byte_Binding.Register(app);
            UnityGameFramework_Runtime_ConfigComponent_Binding.Register(app);
            UGFExtensions_UGuiForm_Binding.Register(app);
            UnityGameFramework_Runtime_VarByte_Binding.Register(app);
            System_Threading_Tasks_Task_1_UIForm_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_UIForm_Binding.Register(app);
            UnityGameFramework_Runtime_UIForm_Binding.Register(app);
            UnityGameFramework_Runtime_LoadConfigSuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadConfigFailureEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadDictionarySuccessEventArgs_Binding.Register(app);
            UnityGameFramework_Runtime_LoadDictionaryFailureEventArgs_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Boolean_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_Type_Binding.Register(app);
            UnityGameFramework_Runtime_LocalizationComponent_Binding.Register(app);
            GameFramework_Resource_LoadAssetCallbacks_Binding.Register(app);
            UnityGameFramework_Runtime_ResourceComponent_Binding.Register(app);
            System_Collections_Generic_ICollection_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_IEnumerable_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_IEnumerator_1_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_Collections_Generic_IDictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_ILTypeInstance_Byte_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Byte_Byte_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_KeyValuePair_2_Byte_Byte_Byte_Array_Binding.Register(app);
            System_Collections_Generic_List_1_Byte_Binding.Register(app);
            System_Enum_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_ValueType_Binding.Register(app);
            UnityEngine_Screen_Binding.Register(app);
            UGFExtensions_CommonButton_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            ComponentAutoBindTool_Binding.Register(app);
            UGFExtensions_DialogParams_Binding.Register(app);
            UnityGameFramework_Runtime_GameEntry_Binding.Register(app);
            UnityEngine_UI_Toggle_Binding.Register(app);
            UnityEngine_Events_UnityEvent_1_Boolean_Binding.Register(app);
            UnityEngine_Events_UnityEvent_1_Single_Binding.Register(app);
            UnityGameFramework_Runtime_SettingComponent_Binding.Register(app);
            UGFExtensions_DataTableExtension_Binding.Register(app);
            System_IO_MemoryStream_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            System_IO_BinaryReader_Binding.Register(app);
            BinaryExtension_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_LinkedList_1_EventHandler_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_LinkedList_1_EventHandler_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_LinkedListNode_1_EventHandler_1_ILTypeInstance_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector3));
            s_UnityEngine_Vector3_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Quaternion));
            s_UnityEngine_Quaternion_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector2));
            s_UnityEngine_Vector2_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2>;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            s_UnityEngine_Vector3_Binding_Binder = null;
            s_UnityEngine_Quaternion_Binding_Binder = null;
            s_UnityEngine_Vector2_Binding_Binder = null;
        }
    }
}
