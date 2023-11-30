namespace DatabaseDesignProject.Models
{
    public class Review
    {
        public int reviewID { get; set; }
        public int review_shoeID { get; set; }
        public string reviewer { get; set; }
        public int rating { get; set; }
        public string rating_description { get; set; }
        
    }
}
