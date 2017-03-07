using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JSON
{
	public class False : Value
	{
        Byte[] myInternalBytes = { Convert.ToByte('f'), Convert.ToByte('a'), Convert.ToByte('l'), Convert.ToByte('s'), Convert.ToByte('e')};
        public False(ref int aPosition)
        {
            aPosition += 5;
        }
        public override void PrettyPrint(Stream aStream, int tabdepth)
        {
            base.doTabDepth(tabdepth);
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            aStream.Write(myInternalBytes, 0, myInternalBytes.Length);
        }
        public override int GetWeight()
        {
            return 1;
        }
        
	}
}
