namespace DatabaseDesignProject.Models
{
    public class ViewCartModel
    {
        public string cart_user { get; set; }
        public List<Shoe> shoes_in_cart { get; set; }
    }
}
