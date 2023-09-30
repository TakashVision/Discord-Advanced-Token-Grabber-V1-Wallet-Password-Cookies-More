using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using BlitzedConfuser.Utils;

namespace BlitzedConfuser.Protections
{
    public class AntiTamper : Protection
    {
        public AntiTamper()
        {
            Name = "Anti-Tamper";
        }

        // Thanks to the EOF Anti-Tamper project by Xenocode on GitHub!

        public static bool Tampered { get; set; }

        public static void Inject(string filePath)
        {
            using (MD5 hash = MD5.Create())
            {
                byte[] bytes = hash.ComputeHash(File.ReadAllBytes(filePath));

                using (FileStream fs = new FileStream(filePath, FileMode.Append))
                    fs.Write(bytes, 0, bytes.Length);
            }
        }

        public override void Execute()
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(TamperClass).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(TamperClass).MetadataToken));
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, Kappa.Module.GlobalType, Kappa.Module);
            MethodDef init = (MethodDef)members.Single(method => method.Name == "NoTampering");
            init.GetRenamed();

            Kappa.Module.GlobalType.FindOrCreateStaticConstructor().Body.Instructions.Insert(0,
                Instruction.Create(OpCodes.Call, init));

            foreach (MethodDef method in Kappa.Module.GlobalType.Methods)
                if (method.Name.Equals(".ctor"))
                {
                    Kappa.Module.GlobalType.Remove(method);
                    break;
                }

            Tampered = true;
        }
    }
}
