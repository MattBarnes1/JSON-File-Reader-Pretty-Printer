using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JSON
{
	public class True : Value
    {
        Byte[] myInternalBytes = { Convert.ToByte('t'), Convert.ToByte('r'), Convert.ToByte('u'), Convert.ToByte('e')};
        public True(ref int aPosition)
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
            
            return "true"; //TODO: check this value
		}
	}
}
