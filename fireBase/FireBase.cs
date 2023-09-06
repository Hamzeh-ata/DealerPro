namespace DealerPro.fireBase
{
    public abstract class FireBase
    {
        public abstract Task<bool> CheckIfAlreadyExists(string endPoint, string storeName, string name, string oldPrice, string price);
        public abstract Task InsertDataIntoFirebase(string endPoint, string storeName, string name, string imageUrl, string oldPrice, string newPrice, string stock, string brand, List<string> texts, string productUrl);
        public abstract Task InsertDataIntoFirebase(string endPoint, string storeName, string name, string img, string oldPrice, string price, string brand, List<string> LiTexts, string productUrl, string productNumber);
        public abstract Task CheckIfElementIsStillExists(string endPoint, string storeName, HashSet<string> storedProducts);
        public abstract Task CheckIfElementIsStillExistsByProductNumber(string endPoint, string storeName, HashSet<string> storedProducts);
        public abstract Task<bool> CheckForDuplicate(string name, string storeName);
        public abstract Task<bool> CheckForDuplicateByProductNumber(string ProductNumber, string StoreName);
        public abstract  Task<bool> CheckForPriceChanges(string name, string storeName, string oldPrice, string price);
        public abstract Task<bool> CheckForPriceChangesByProductNumber(string productNumber, string storeName, string oldPrice, string price);
    }
}
