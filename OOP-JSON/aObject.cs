using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JSON
{
	public class aObject : Value
	{
		Dictionary<String, Value> myPair = new Dictionary<String, Value>();

		public override int GetWeight()
		{
            int Amount = 1;
            if (myPair.Count > 0)
            {
                var anEnum = myPair.GetEnumerator();
                while (anEnum.MoveNext())
                {
                    Amount += anEnum.Current.Value.GetWeight();
                }
            }
            return Amount;
        }

        Byte[] valueColonOffset = { Convert.ToByte(':'), Convert.ToByte(' ') };
        Byte myInternalPar = Convert.ToByte('{');
        Byte myInternalParEnd =  Convert.ToByte('}');
        Byte[] newValueCommaOffset = { Convert.ToByte(','), Convert.ToByte('\n') };
        Byte[] emptyObject = { Convert.ToByte('{'), Convert.ToByte(' '), Convert.ToByte('}') };
        Byte myInternalByte = Convert.ToByte('\n');
        public override void PrettyPrint(Stream aStream, int tabdepth)
        {
            
            base.doTabDepth(tabdepth);
            aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
            if (myPair.Count > 0)
            {
                var anEnum = myPair.GetEnumerator();
                bool hasNext = anEnum.MoveNext();
                aStream.WriteByte(myInternalPar);
                aStream.WriteByte(myInternalByte);
                while (hasNext)
                {
                    anEnum.Current.Key.PrettyPrint(aStream, tabdepth + 1);
                    aStream.Write(valueColonOffset, 0, valueColonOffset.Length);
                    if (anEnum.Current.Value is array)
                    {
                            aStream.WriteByte(myInternalByte);
                            anEnum.Current.Value.PrettyPrint(aStream, tabdepth + 1);
                    }
                    else if (anEnum.Current.Value is aObject)
                    {
                        aStream.WriteByte(myInternalByte);
                        anEnum.Current.Value.PrettyPrint(aStream, tabdepth + 1);
                    }
                    else
                    {
                        anEnum.Current.Value.PrettyPrint(aStream, 0);
                    }
                    hasNext = anEnum.MoveNext();
                    if (hasNext)
                    {
                        aStream.Write(newValueCommaOffset, 0, newValueCommaOffset.Length);
                    }
                }
                aStream.WriteByte(myInternalByte);
                aStream.Write(base.myByteDepth, 0, base.myByteDepth.Length);
                aStream.WriteByte(myInternalParEnd);
            }
            else if (myPair.Count == 0)
            {
                aStream.Write(emptyObject, 0, emptyObject.Length);
            }

        }

#pragma warning disable CS0414 // The field 'aObject.firstObject' is assigned but its value is never used
      //  bool firstObject = false;
#pragma warning restore CS0414 // The field 'aObject.firstObject' is assigned but its value is never used

        public aObject(System.String aString, ref int aPosition, bool namelessObject)
        {
            //if (aPosition == 0)
           // {
            //    firstObject = true;
            //}
            aPosition++;
            doValuePair(aString, ref aPosition);
        }

        private void doKeyValuePair(System.String aString, ref int aPosition)
        {
            String myKeyString;
            if(aString[aPosition].Equals(' ') || aString[aPosition].Equals(':'))
            {
                aPosition++;
                doKeyValuePair(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals('\"'))
            {
                myKeyString = new JSON.String(aString, ref aPosition);
                Value aValue = doValuePair(aString, ref aPosition);
                myPair.Add(myKeyString, aValue);
                doKeyValuePair(aString, ref aPosition);
            }
        }

        bool hasValue = false;
       

        private Value doValuePair(System.String aString, ref int aPosition)
        {
#pragma warning disable CS0168 // The variable 'result' is declared but never used
            int result;
#pragma warning restore CS0168 // The variable 'result' is declared but never used
            if (aString[aPosition].Equals('\"'))
            {
                if (hasValue)
                {
                    return new JSON.String(aString, ref aPosition);
                }
                else
                {
                    hasValue = true;
                    JSON.String aName = new JSON.String(aString, ref aPosition);
                    myPair.Add(aName, doValuePair(aString, ref aPosition));
                    hasValue = false;
                    return doValuePair(aString, ref aPosition);
                }
            }
            else if (aString[aPosition].Equals('['))
            {
                return new array(aString, ref aPosition); //Will break this function if all chars don't handle their closing items
            }
            else if (aString[aPosition].Equals('{'))
            {
                return new aObject(aString, ref aPosition, false);
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
            else if (char.IsNumber(aString[aPosition]) || aString[aPosition].Equals('-') || aString[aPosition] == '+')//TODO: char.isnumber
            {
                return new number(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals(' ') || aString[aPosition].Equals('\r') || aString[aPosition].Equals('\n'))
            {
                aPosition++;
                return doValuePair(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals(':'))
            {
                aPosition++;
                return doValuePair(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals(','))
            {
                aPosition++;
                return doValuePair(aString, ref aPosition);
            }
            else if (char.IsDigit(aString[aPosition]))
            {
                return new number(aString, ref aPosition);
            }
            else if (aString[aPosition].Equals('}'))
            {
                if (hasValue)
                {
                    throw new Exception("Closing bracket detected when value was expected at position: " + aPosition);
                }
                aPosition++;
                return null;
            }
                Console.Write("Unhandled Character: " + aString[aPosition] + " at character position: " + aPosition + "Substring: " + aString.Substring(aPosition, 40));
            throw new Exception("Unhandled Character: " + aString[aPosition] + " at character position: " + aPosition);
        }

    }
}
