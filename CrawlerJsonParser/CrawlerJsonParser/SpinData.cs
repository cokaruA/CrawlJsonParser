using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CrawlerJsonParser
{
    public class ReelData
    {
        public struct StripInfo
        {
            public string reel_set;
            public string s ;
            public string sa;
            public string sb;
            public string sh;
            public string st;
            public string sw;
        };


        public StripInfo reg = new  StripInfo();
        public StripInfo top = new StripInfo(); 

    }

    public  class SpinData
    {
        public Dictionary<string, string> datum = new Dictionary<string, string>();
        //Dictionary<string, string> spinDatum = new Dictionary<string, string>();

        public SpinData()
        {
            foreach (string _key in Enum.GetNames(typeof(SpinDataKey)))
            {
                datum.Add(_key, string.Empty);
            }

            //foreach(string _key in Enum.GetNames(typeof(ReelDataKey)))
            //{
            //    spinDatum.Add(_key, string.Empty);
            //}
        }

        public string ToCSVString()
        {
            List<char> charsToRemove = new List<char>() { ',' };
            StringBuilder sb = new StringBuilder();
            foreach (string _key in Enum.GetNames(typeof(SpinDataKey)))
            {                
                string value = datum[_key];
                if (IsMergeNumbers(_key))
                {
                    value = Filter(value, charsToRemove, string.Empty);
                }
                else if (IsLinePayData(_key))
                {
                    string newValue = " | ";
                    value = Filter(value, charsToRemove, newValue);
                }
                else if (IsSlm(_key))
                {
                    string newValue = " | ";
                    value = Filter(value, charsToRemove, newValue);
                }
                else if(IsJsonObject(_key))
                {
                    value = ParsingJsonObject(value);   
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
               key.CompareTo("balance_cash") == 0 ||
               key.CompareTo("fswin") == 0 ||
               key.CompareTo("fsres") == 0 ||
               key.CompareTo("fsres_total") == 0 ||
               key.CompareTo("fswin_total") == 0 ||
               key.CompareTo("tw") == 0 ||
               key.CompareTo("w") == 0
               )
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

        private bool IsSlm(string key)
        {
            if (key.CompareTo("slm_mp") == 0 ||
                key.CompareTo("slm_mv") == 0 ||
                key.CompareTo("slm_lmi") == 0 ||
                key.CompareTo("slm_lmv") == 0
                )
            {
                return true;
            }

            return false;
        }

        private bool IsJsonObject(string key)
        {
            if(key.CompareTo("g") == 0 )
            {
                return true;
            }

            return false;
        }

        private string Filter(string str, List<char> charsToRemove, string newValue)
        {
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), newValue));
            return str;
        }

        private string ParsingJsonObject(string str)
        {
            ReelData reelData = (ReelData)JsonConvert.DeserializeObject<ReelData>(str);
            Debug.Assert( reelData != null );   

            if(reelData == null )
            {
                return string.Empty;
            }

            string stripInfo = string.Empty;
            stripInfo = ParsingStripInfo(reelData.reg);
            stripInfo += ",";
            stripInfo += ParsingStripInfo(reelData.top);

            return stripInfo;
        }


        private string ParsingStripInfo(ReelData.StripInfo strip)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ReelDataKey _key in Enum.GetValues(typeof(ReelDataKey)))
            {
                if (_key == ReelDataKey.reel_set)
                {
                    sb.Append(strip.reel_set.ToString());
                }
                else if (_key == ReelDataKey.s)
                {
                    sb.Append(strip.s.ToString());
                }
                else if (_key == ReelDataKey.sa)
                {
                    sb.Append(strip.sa.ToString());
                }
                else if (_key == ReelDataKey.sb)
                {
                    sb.Append(strip.sb.ToString());
                }
                else if (_key == ReelDataKey.sh)
                {
                    sb.Append(strip.sb.ToString());
                }
                else if (_key == ReelDataKey.st)
                {
                    sb.Append(strip.st.ToString());
                }
                else if (_key == ReelDataKey.sw)
                {
                    sb.Append(strip.sw.ToString());
                }

                if (_key != ReelDataKey.sw)
                {
                    sb.Append(',');
                }
            }

            return sb.ToString();
        }

        private string ToReelDataString(ReelDataSet dataSet, ReelData.StripInfo stripInfo )
        {



            return string.Empty;
        }


    }



}
