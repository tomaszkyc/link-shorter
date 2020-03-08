using System;
using System.Runtime.Serialization;

namespace LinkShorter.Models.Dashboard.Statistics
{
    public class DataPoint
    {

        [DataMember(Name = "data")]
        public Nullable<long> _data;

        [DataMember(Name = "category")]
        public string _category;

        public DataPoint()
        {
        }

        public DataPoint(long data, string category)
        {
            this._data = data;
            this._category = category;
        }


    }
}
