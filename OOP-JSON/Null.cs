using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JSON
{
	public class Null : Value
    {
        Byte[] myInternalBytes = { Convert.ToByte('n'), Convert.ToByte('u'), Convert.ToByte('l'), Convert.ToByte('l') };
        public Null(ref int aPosition)
        {
            aPosition += 4;
        }
        public override int GetWeight()
        {
            return 1;
        }
        public override void PrettyPrint(Stream aStream, int tabdepth)
        {
            base.doTabDepth(tabdepth);
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            aStream.Write(myInternalBytes, 0, myInternalBytes.Length);
        }
        public override string ToString()
		{
            return "null";
		}
	}
}
