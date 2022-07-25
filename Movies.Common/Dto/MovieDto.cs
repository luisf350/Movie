using System;

namespace Movies.Common.Dto
{
    public class MovieDto : BaseDto
    {
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Director { get; set; }
        public DateTime Released { get; set; }
        public bool IsPrivate { get; set; }
        public Guid UserId { get; set; }
    }
}
