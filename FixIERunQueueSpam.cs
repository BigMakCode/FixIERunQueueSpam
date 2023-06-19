using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
namespace Oxide.Plugins
{
    [Info("FixIERunQueueSpam", "bmgjet", "1.0.0")]
    [Description("Stops RunQueue Spam")]
    class FixIERunQueueSpam : RustPlugin
    {
        //Harmony Patch
        private string HarmonyPatchData = "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dGlvbkF0dHJpYnV0ZQBBc3NlbWJseURlc2NyaXB0aW9uQXR0cmlidXRlAENvbXBpbGF0aW9uUmVsYXhhdGlvbnNBdHRyaWJ1dGUAQXNzZW1ibHlQcm9kdWN0QXR0cmlidXRlAEFzc2VtYmx5Q29weXJpZ2h0QXR0cmlidXRlAEFzc2VtYmx5Q29tcGFueUF0dHJpYnV0ZQBSdW50aW1lQ29tcGF0aWJpbGl0eUF0dHJpYnV0ZQBTeXN0ZW0uUnVudGltZS5WZXJzaW9uaW5nAFN0cmluZwBIYXJtb255UGF0Y2gAQWRkUXVldWVOdWxsQ2hlY2sAQWRkUXVldWVOdWxsQ2hlY2suZGxsAGdldF9JdGVtAFN5c3RlbQBTeXN0ZW0uUmVmbGVjdGlvbgBDb2RlSW5zdHJ1Y3Rpb24ATm9wAEFzc2VtYmx5LUNTaGFycABTeXN0ZW0uTGlucQBUcmFuc3BpbGVyAFNlcnZlck1ncgAuY3RvcgBMZHN0cgBTeXN0ZW0uRGlhZ25vc3RpY3MAU3lzdGVtLlJ1bnRpbWUuSW50ZXJvcFNlcnZpY2VzAFN5c3RlbS5SdW50aW1lLkNvbXBpbGVyU2VydmljZXMAT3BDb2RlcwBEZWJ1Z2dpbmdNb2RlcwBpbnN0cnVjdGlvbnMAT2JqZWN0AFN5c3RlbS5SZWZsZWN0aW9uLkVtaXQAZ2V0X0NvdW50AFRvTGlzdAAwSGFybW9ueQBvcF9FcXVhbGl0eQAAAAAAV1MAZQByAHYAZQByACAARQB4AGMAZQBwAHQAaQBvAG4AOgAgAEkAbgBkAHUAcwB0AHIAaQBhAGwARQBuAHQAaQB0AHkALgBSAHUAbgBRAHUAZQB1AGUAAAAAAAK7jN67ddpFqqxJPmH4mokABCABAQgDIAABBSABARERBCABAQ4EIAEBAgYgAgESQQ4JBwIVElkBElUIDxABARUSWQEeABUSUQEeAAQKARJVBhUSWQESVQUgARMACAMGEWEHAAICEWERYQIGHAUAAgIODgMgAAgIt3pcVhk04IkOAAEVElEBElUVElEBElUIAQAIAAAAAAAeAQABAFQCFldyYXBOb25FeGNlcHRpb25UaHJvd3MBCAEAAgAAAAAAFgEAEUFkZFF1ZXVlTnVsbENoZWNrAAAFAQAAAAAXAQASQ29weXJpZ2h0IMKp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VgBlAHIAcwBpAG8AbgAAADEALgAwAC4AMAAuADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAAwAAAB8OgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
        private string HarmonyPath = "./HarmonyMods/AddQueueNullCheck.dll";
        private string PatchName = "AddQueueNullCheck";

        //Reflection
        private MethodInfo WriteBytes;
        private MethodInfo DeleteFile;
        private MethodInfo Exists;
        private class Container<T> { public readonly T Value; public Container(T Value) { this.Value = Value; } }
        private T GetProperty<T>(object obj, string property) { var info = obj.GetType().GetProperty(property, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance); return (T)info?.GetValue(obj); }

        private MethodInfo StealthGetMethod(string definingLib, string declaringType, string methodName, Type[] args)
        {
            Container<Assembly[]> assemblies = new Container<Assembly[]>(AppDomain.CurrentDomain.GetAssemblies());
            Container<Assembly> target = null;
            for (var index = 0; index < assemblies.Value.Length; index++) { if (GetProperty<string>(assemblies.Value[index], "FullName").StartsWith(definingLib)) { target = new Container<Assembly>(assemblies.Value[index]); } }
            return GetProperty<IEnumerable<TypeInfo>>(target.Value, "DefinedTypes").FirstOrDefault(x => x.Name == declaringType).GetMethod(methodName, args); ;
        }

        private void Init()
        {
            //Set up reflection
            WriteBytes = StealthGetMethod("mscorlib", "File", "WriteAllBytes", new Type[] { typeof(string), typeof(byte[]) });
            DeleteFile = StealthGetMethod("mscorlib", "File", "Delete", new Type[] { typeof(string) });
            Exists = StealthGetMethod("mscorlib", "File", "Exists", new Type[] { typeof(string) });
            // Create Harmony DLL
            if (!(bool)Exists.Invoke(null, new object[] { HarmonyPath }))
            {
                WriteBytes.Invoke(null, new object[] { HarmonyPath, Convert.FromBase64String(HarmonyPatchData) });
                ConsoleSystem.Run(ConsoleSystem.Option.Server.Quiet(), "harmony.load " + PatchName, Array.Empty<object>()); //Call load on new harmony dll
            }
        }

        void Unload() { if ((bool)Exists.Invoke(null, new object[] { HarmonyPath })) { ConsoleSystem.Run(ConsoleSystem.Option.Server.Quiet(), "harmony.unload " + PatchName, Array.Empty<object>()); DeleteFile.Invoke(null, new object[] { HarmonyPath }); } }
    }
}