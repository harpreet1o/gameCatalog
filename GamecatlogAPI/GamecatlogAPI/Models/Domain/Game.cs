namespace GamecatalogAPI.Models.Domain
{
    public class Game
    {
        // used Guid to generate unique values so the user can't know the amount of games in db
        public Guid Id { get; set; }
        // will try to break this to learn by avoiding the string.Empty and no required keyword 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public string Genre { get; set; }
        //used the ? to make it nullable if the user doessn't want to add image
        public string? GameImageURL { get; set; }
    }
}
