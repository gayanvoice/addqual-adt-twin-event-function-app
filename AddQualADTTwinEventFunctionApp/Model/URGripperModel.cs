namespace AddQualADTTwinEventFunctionApp.Model
{
    public class URGripperModel
    {
        public DataModel data { get; set; }
        public string dataschema { get; set; }
        public string contenttype { get; set; }
        public string traceparent { get; set; }
        public class DataModel
        {
            public int ACT { get; set; }
            public int GTO { get; set; }
            public int FOR { get; set; }
            public int SPE { get; set; }
            public int POS { get; set; }
            public int STA { get; set; }
            public int PRE { get; set; }
            public int OBJ { get; set; }
            public int FLT { get; set; }
        }
    }
}