using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerJsonParser
{
    public  class SpinData
    {
        public Dictionary<string, string> datum = new Dictionary<string, string>();

        public SpinData()
        {
            foreach (string _key in Enum.GetNames(typeof(SpinDataKey)))
            {
                datum.Add(_key, string.Empty);
            }
        }

        public string ToCSVString()
        {
            List<char> charsToRemove = new List<char>() { ',' };
            StringBuilder sb = new StringBuilder();
            foreach (string _key in Enum.GetNames(typeof(SpinDataKey)))
            {                
                string value = datum[_key];
                if(IsMergeNumbers(_key))
                {
                    value = Filter(value, charsToRemove,string.Empty);
                }
                else if(IsLinePayData(_key))
                {
                    string newValue = " | ";
                    value = Filter(value, charsToRemove, newValue);
                }

                sb.Append(value);
                if(_key.CompareTo("w") !=0 )
                {
                    sb.Append(',');
                }
            }

            return sb.ToString();
        }


        private bool IsMergeNumbers(string key)
        {
            if(key.CompareTo("balance") == 0 ||
               key.CompareTo("balance_bonus") ==0 ||
               key.CompareTo("balance_cash") == 0)
            {
                return true;
            }

            return false;
        }

        private bool IsLinePayData(string key)
        {
            if (key.CompareTo("linepay") == 0 )
            {
                return true;
            }

            return false;
        }

        private string Filter(string str, List<char> charsToRemove, string newValue)
        {
            //Ha Kee Si Ru!! Baldus3 Jo A 
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), newValue));
            return str;
        }

    }
}
