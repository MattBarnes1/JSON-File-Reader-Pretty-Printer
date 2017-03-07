using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace JSON
{
	public class array : Value
	{
        List<Value> myPair = new List<Value>();
        Byte StartValue = Convert.ToByte('[');
        Byte EndValue = Convert.ToByte(']');
        Byte[] newValueCommaOffset = { Convert.ToByte(','), Convert.ToByte('\n') };
        Byte[] newValueCommaNoOffset = { Convert.ToByte(','), Convert.ToByte(' ') };
        Byte newValueNoCommaOffset = Convert.ToByte('\n');
        Byte[] emptyArray = { Convert.ToByte('['), Convert.ToByte(' '), Convert.ToByte(']') };
        public override int GetWeight()
		{
            var ReturnValue = 1;
            foreach(Value aVal in myPair)
            {
                ReturnValue += aVal.GetWeight();
            }
            return ReturnValue;
        }
        public override void PrettyPrint(Stream aStream, int tabdepth)
        {
            base.doTabDepth(tabdepth);
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            if (myPair.Count == 0)
            {
                aStream.Write(emptyArray, 0, emptyArray.Length);
                return;
            }
            aStream.WriteByte(StartValue);
            aStream.WriteByte(newValueNoCommaOffset);

            for (int i = 0; i < myPair.Count; i++)
            {
                myPair[i].PrettyPrint(aStream, tabdepth + 1);
                if((i + 1) == myPair.Count)
                {
                        aStream.WriteByte(newValueNoCommaOffset);
                }
                else
                {
                        aStream.Write(newValueCommaOffset, 0, newValueCommaNoOffset.Length);
                }
            }
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            aStream.WriteByte(EndValue);
        }
        public int getLength()
        {
            return myPair.Count;
        }

        private Value doValuePair(System.String aString, ref int aPosition)
        {
#pragma warning disable CS0168 // The variable 'result' is declared but never used
            int result;
#pragma warning restore CS0168 // The variable 'result' is declared but never used
            if (aString[aPosition].Equals('\"'))
            {
                return new JSON.String(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals('['))
            {
                return new array(aString, ref aPosition); //Will break this function if all chars don't handle their closing items
            }
            else if (aString[aPosition].Equals('{'))
            {
                return new aObject(aString, ref aPosition, true);
            }
            else if (aString[aPosition].Equals('n'))
            {
                return new Null(ref aPosition);
            }
            else if (aString[aPosition].Equals('f'))
            {
                return new False(ref aPosition);
            }
            else if (aString[aPosition].Equals('t'))
            {
                return new True(ref aPosition);
            }
            else if (char.IsNumber(aString[aPosition]) || aString[aPosition].Equals('-') || aString[aPosition] == '+')
            {
                return new number(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals(' ') || aString[aPosition].Equals('\r') || aString[aPosition].Equals('\n'))
            {
                aPosition++;
                return doValuePair(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals(','))
            {
                aPosition++;
                return doValuePair(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals(']'))
            {
                aPosition++;
                return null;
            }
            Console.Write("Unhandled Character: " + aString[aPosition] + " at character position: " + aPosition + "Substring: " + aString.Substring(aPosition, 40));
            throw new Exception("Unhandled Character: " + aString[aPosition] + " at character position: " + aPosition);
        }
        public array(System.String aString, ref int aPosition)
		{
            aPosition++; //Increment to move us past the first value;
            while(aString[aPosition] != ']')
            {
                Value aVal = doValuePair(aString, ref aPosition);
                if (aVal == null) break; //Used to prevent empty arrays from adding a bad char
                myPair.Add(aVal);
            }
            aPosition++;
		}
	}
}
