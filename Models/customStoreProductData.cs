namespace DealerPro.Models
{
    public class customStoreProductData
    {
        public string UID { get; set; }
        public string StoreName { get; set; }
        public string Name { get; set; }
        public IFormFile Img { get; set; }
        public string Category { get; set; }
        public string Image  { get; set; }
        public string Price { get; set; }
        public string OldPrice { get; set; }
        public string Brand { get; set; }
        public List<string> description { get; set; }
        public string ProductUrl { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string Time { get; set; } = DateTime.Now.ToString("HH:mm");



    }
}
