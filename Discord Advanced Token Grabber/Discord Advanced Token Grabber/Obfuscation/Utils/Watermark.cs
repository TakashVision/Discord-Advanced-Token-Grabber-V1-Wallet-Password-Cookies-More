﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace BlitzedConfuser.Utils
{
    public class Watermark
    {
        public static void AddAttribute()
        {
            TypeRef attrRef = Kappa.Module.CorLibTypes.GetTypeRef("System", "Attribute");
            TypeDefUser attrType = new TypeDefUser(string.Empty, "BlitzedAttribute", attrRef);
            Kappa.Module.Types.Add(attrType);
            MethodDefUser ctor = new MethodDefUser(".ctor", MethodSig.CreateInstance(Kappa.Module
                .CorLibTypes.Void, Kappa.Module.CorLibTypes.String), MethodImplAttributes.Managed,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName)
            {
                Body = new CilBody { MaxStack = 1 }
            };

            ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            ctor.Body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(Kappa.Module, ".ctor",
                MethodSig.CreateInstance(Kappa.Module.CorLibTypes.Void), attrRef)));
            ctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            attrType.Methods.Add(ctor);

            CustomAttribute attr = new CustomAttribute(ctor);
            attr.ConstructorArguments.Add(new CAArgument(Kappa.Module.CorLibTypes.String, $"Obfuscated" +
                $" with {Reference.Name} version {Reference.Version}."));
            Kappa.Module.CustomAttributes.Add(attr);
        }
    }
}
