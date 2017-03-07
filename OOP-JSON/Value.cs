using System;
using System.Collections.Generic;
using System.Text;

namespace JSON
{
	public abstract class Value
	{
        public abstract void PrettyPrint(System.IO.Stream aStream, int tabdepth);
		public abstract int GetWeight();

        private Byte Tab = Convert.ToByte('\t');
        protected Byte[] myByteDepth = { };
        protected void doTabDepth(int tabDepth)
        {
            myByteDepth = new Byte[tabDepth];
            for (int i = 0; i < tabDepth; i++)
            {
                myByteDepth[i] = Tab;
            }
        }
	}
}
