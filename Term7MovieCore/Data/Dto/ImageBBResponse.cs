namespace Term7MovieCore.Data.Dto
{
    public class ImageBBResponse
    {
        public ImageBBResponseData Data { set; get; }
        public bool Success { set; get; }
        public int Status { set; get; }
    }
    public class ImageBBResponseData
    {
        public string Id { set; get; }
        public string Title { set; get; }
        public string UrlViewer { set; get; }
        public string Url { set; get; }
        public string DisplayUrl { set; get; }
        public string Width { set; get; }
        public string Height { set; get; }
        public string Size { set; get; }
        public string Time { set; get; }
        public string Expiration { set; get; }
        public ImageData Image { set; get; }
    }

    public class ImageData
    {
        public string FileName { set; get; }
        public string Name { set; get; }
        public string Mime { set; get; }
        public string Extension { set; get; }
        public string Url { set; get; }
    }
}
