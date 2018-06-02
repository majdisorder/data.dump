namespace Data.Dump.Persistence
{
    public class TablePair
    {
        public TablePair(string liveTable, string tempTable)
        {
            LiveTable = liveTable;
            TempTable = tempTable;
        }
        public string LiveTable { get; set; }
        public string TempTable { get; set; }
    }
}
