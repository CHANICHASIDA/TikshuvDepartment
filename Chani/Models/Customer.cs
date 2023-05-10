namespace TikshuvProject.Models
{
    public class Customer
    {
        public string id { get; set; } 
        public string firstName { get; set; }
        public string lastName  { get; set; }

        public string address { get; set; } 
        public string city { get; set; }
            
        public int numOfStreet { get; set; }
            
        public string phone { get; set; }
            
        public string mobile { get; set; }
                        
        public DateTime birthday { get; set; }
    }
}
