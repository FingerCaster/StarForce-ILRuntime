using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using ILRuntime.Runtime.Intepreter;
using LitJson;
using UnityEngine;

namespace UGFExtensions
{
    public class ProcedureEnterHotfix : ProcedureBase
    {
        public override bool UseNativeDialog => false;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            List<MyData> myDatas = new List<MyData>();
            myDatas.Add(new MyData(1,1,1){JsonData = new Dictionary<string ,int>(){{"1",1}} });
            myDatas.Add(new MyData(2,2,2){JsonData = new Dictionary<string ,int>(){{"2",2}}});
            string str = JsonMapper.ToJson(myDatas);
            Debug.Log("---------------");
            Debug.Log(str);
            Debug.Log("---------------");
            List<MyData> myDatas1 = JsonMapper.ToObject<List<MyData>>(str);
            foreach (var item in myDatas1)
            {
                Debug.Log(item.x + ""+ item.z+item.configId+item.JsonData.ToString());
            }
            base.OnEnter(procedureOwner);
            GameEntry.Hotfix.Enter();
        }
        class MyData
        {
            public int x;
            public int z;
            public int configId;
            public object JsonData;
            public MyData()
            {
            }

            public MyData(int x, int z, int configId)
            {
                this.x = x;
                this.z = z;
                this.configId = configId;
            }
        }
    }
}