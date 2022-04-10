using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class TestJson
    {
        public TestBase TestBase1 { get; set; }
        public TestBase TestBase;
        public List<TestBase> TestList;
        public List<TestBase> TestList1 { get; set; }
        public TestBase[] TestArray1 { get; set; }
        public TestBase[] TestArray;
        public Dictionary<int, TestBase> TestDictionary1 { get; set; }
        public Dictionary<int, TestBase> TestDictionary;
    }

    public class TestMono
    {
        public void Start()
        {
            TestJson testJson = new TestJson()
            {
                TestBase = new TestSon1()
                {
                    BaseInt = 1,
                    BaseInt1 = 2,
                    Son = 3,
                    SonP = 4,
                },
                TestBase1 = new TestSon1()
                {
                    BaseInt = 5,
                    BaseInt1 = 6,
                    Son = 7,
                    SonP = 8,
                },
                TestList = new List<TestBase>()
                {
                    new TestBase()
                    {
                        BaseInt = 1,
                        BaseInt1 = 2,
                    },
                    new TestSon1()
                    {
                        BaseInt = 3,
                        BaseInt1 = 4,
                        Son = 5,
                        SonP = 6
                    },
                },
                TestList1 = new List<TestBase>()
                {
                    new TestBase()
                    {
                        BaseInt = 7,
                        BaseInt1 = 8,
                    },
                    new TestSon1()
                    {
                        BaseInt = 9,
                        BaseInt1 = 10,
                        Son = 11,
                        SonP = 12
                    },
                },
                TestDictionary = new Dictionary<int, TestBase>()
                {
                    {
                        1, new TestBase()
                        {
                            BaseInt = 13,
                            BaseInt1 = 14,
                        }
                    },
                    {
                        2, new TestSon1()
                        {
                            BaseInt = 15,
                            BaseInt1 = 16,
                            Son = 17,
                            SonP = 18
                        }
                    },
                },
                TestDictionary1 = new Dictionary<int, TestBase>()
                {
                    {
                        1, new TestBase()
                        {
                            BaseInt = 19,
                            BaseInt1 = 20,
                        }
                    },
                    {
                        2, new TestSon1()
                        {
                            BaseInt = 21,
                            BaseInt1 = 22,
                            Son = 23,
                            SonP = 24
                        }
                    },
                },
                TestArray = new TestBase[]
                {
                    new TestBase()
                    {
                        BaseInt = 25,
                        BaseInt1 = 26,
                    },
                    new TestSon1()
                    {
                        BaseInt = 27,
                        BaseInt1 = 28,
                        Son = 29,
                        SonP = 30
                    },
                },
                TestArray1 = new TestBase[]
                {
                    new TestBase()
                    {
                        BaseInt = 31,
                        BaseInt1 = 32,
                    },
                    new TestSon1()
                    {
                        BaseInt = 33,
                        BaseInt1 = 34,
                        Son = 35,
                        SonP = 36
                    },
                },
                
            };
            var json = CatJson.JsonParser.ToJson(testJson);
            Log.Warning("testjson:  " + json);

            TestJson testJson1 = CatJson.JsonParser.ParseJson<TestJson>(json);
            Log.Warning("testjson1:  " + CatJson.JsonParser.ToJson(testJson1));

            TestBase[] testBases = new TestBase[]
            {
                new TestBase()
                {
                    BaseInt = 31,
                    BaseInt1 = 32,
                },
                new TestSon1()
                {
                    BaseInt = 33,
                    BaseInt1 = 34,
                    Son = 35,
                    SonP = 36
                },
            };
            Log.Warning("testjson1:  " + CatJson.JsonParser.ToJson(testBases));

            List<TestBase> testList = new List<TestBase>
            {
                new TestBase()
                {
                    BaseInt = 31,
                    BaseInt1 = 32,
                },
                new TestSon1()
                {
                    BaseInt = 33,
                    BaseInt1 = 34,
                    Son = 35,
                    SonP = 36
                },
            };
            Log.Warning("testjson1:  " + CatJson.JsonParser.ToJson(testList));

            
        }
    }
}