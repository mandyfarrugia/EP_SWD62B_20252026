namespace Presentation.Models
{
    /* Role of Models: To carry data from Point A to Point B. 
     * - Point A (View) to Point B (Controller)
     * - Point A (Controller) to Point B (View) */
    public class TestModel
    {
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime DatePublished { get; set; }
    }
}