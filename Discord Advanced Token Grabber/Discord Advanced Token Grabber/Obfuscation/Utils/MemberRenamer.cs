using dnlib.DotNet;

namespace BlitzedConfuser.Utils
{
    public static class MemberRenamer
    {
        // Thanks to the AsStrongAsFuck project!
        public static void GetRenamed(this IMemberDef member)
        {
            member.Name = "StvnedEagleWINNINGLOL" + Randomizer.String(45);
        }

        public static int StringLength()
        {
            return Randomizer.Next(120, 30);
        }
    }
}
