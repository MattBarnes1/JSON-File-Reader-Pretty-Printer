using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JSON
{
	public class String : Value
	{
        static StringBuilder myStringBuilder = new StringBuilder();
        string myString = "";
        byte myStartEndQuote =  Convert.ToByte('"');

        public String(string aString, ref int aPosition)
        {
            aPosition++; //Moves us past the first "
                         //while((!(aString[aPosition].Equals('"') && !aString[aPosition-1].Equals('\\'))) && !((aString[aPosition - 1].Equals('\\') && aString[aPosition - 2].Equals('\\') && aString[aPosition].Equals('\"'))))//as long as we see " and not \" just copy EDIT: No, \\" would be allowable
            while (aString[aPosition] != '\"')
            { //TODO: Test these cases
                if (aString[aPosition] == '\\')
                {
                    myStringBuilder.Append(aString[aPosition++]);
                }
                myStringBuilder.Append(aString[aPosition++]);
            }
            myString = myStringBuilder.ToString();
            myStringBuilder.Clear();
            aPosition++; //Moves us past the final " so we don't activate the string again;
        }


        public override void PrettyPrint(Stream aStream, int tabdepth)
        {
            base.doTabDepth(tabdepth);
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            aStream.WriteByte(myStartEndQuote);
            for(int i = 0; i < myString.Length; i++)
            {
                aStream.WriteByte(Convert.ToByte(myString[i]));

            }
            aStream.WriteByte(myStartEndQuote);
        }

        public override int GetWeight()
		{
            return 1;
		}

	}
}
