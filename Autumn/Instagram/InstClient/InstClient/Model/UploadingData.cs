namespace InstClient.Model
{
    public class UploadingData
    {
        public string Filter { get; set; }
        public Pict Picture { get; set; }

        public UploadingData(Pict pict, string filter)
        {
            Filter = filter;
            Picture = pict;
        }
    }
}