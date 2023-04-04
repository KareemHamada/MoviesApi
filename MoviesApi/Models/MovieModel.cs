﻿namespace MoviesApi.Models
{
    public class MovieModel
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public IFormFile Poster { get; set; }
        public byte GenreId { get; set; }
    }
}
