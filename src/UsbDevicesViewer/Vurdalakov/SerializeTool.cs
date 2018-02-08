namespace Vurdalakov
{
    using System;
    using System.Runtime.InteropServices;

    // Based on:
    // https://stackoverflow.com/a/14125895
    public static class SerializeTool
    {
        public static T Deserialize<T>(Byte[] source, Int32 startIndex = 0)
        {
            var size = Marshal.SizeOf(typeof(T));
            if (size > (source.Length - startIndex))
            {
                throw new ArgumentException("Not enough data");
            }

            var buffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(source, startIndex, buffer, size);

            var structure = (T)Marshal.PtrToStructure(buffer, typeof(T));

            Marshal.FreeHGlobal(buffer);

            return structure;
        }

        public static Byte[] Serialize(Object structure)
        {
            var size = Marshal.SizeOf(structure);
            var buffer = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(structure, buffer, false);
            var result = new Byte[size];
            Marshal.Copy(buffer, result, 0, size);

            Marshal.FreeHGlobal(buffer);

            return result;
        }
    }
}
