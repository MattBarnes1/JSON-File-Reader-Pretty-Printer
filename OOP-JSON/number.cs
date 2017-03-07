using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JSON
{
	public class number : Value
	{
		string myValue = "";
        static StringBuilder myStringBuilder = new StringBuilder();
        public number(string aString, ref int aPosition)
        {
            while (char.IsNumber(aString[aPosition]) || (aString[aPosition] == 'e' || aString[aPosition] == 'E' || aString[aPosition] == '.' || aString[aPosition] == '-' || aString[aPosition] == '+'))
            {
                myStringBuilder.Append(aString[aPosition]);
                aPosition++;
            }
            myValue = myStringBuilder.ToString();
            myStringBuilder.Clear();
            //don't need to inc pos for this because it's done in for loop
        }
        public override void PrettyPrint(Stream aStream, int tabdepth)
        {
            base.doTabDepth(tabdepth);
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            foreach (char a in myValue)
            {
                aStream.WriteByte(Convert.ToByte(a));
            }
        }
        public override int GetWeight()
        {
            return 1;
        }

	}
}
