using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

// Exemplos copiado deste site
// http://stevenhollidge.blogspot.com.br/2012/04/servicestack-rest-with-protobuf.html

namespace PrimeiroTesteComServiceStack.Rest
{
    [Description("GET or DELETE a single movie by Id. Use POST to create a new Movie and PUT to update it")]
    public class Movie
    {
        public Movie()
        {
            this.Genres = new List<string>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Rating { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> Genres { get; set; }
    }

    public class MovieResponse
    {
        public Movie Movie { get; set; }
    }

    public class MovieService : RestServiceBase<Movie>
    {
        public static List<Movie> _moviesList = new List<Movie>();


        /// GET /movies/{Id} 
        public override object OnGet(Movie movie)
        {
            // normally you would return a movie from the db
            return new MovieResponse { Movie = movie };
        }

        /// POST /movies
        /// 
        /// returns HTTP Response => 
        ///     201 Created
        ///     Location: http://localhost/ServiceStack.MovieRest/movies/{newMovieId}
        ///     
        ///     {newMovie DTO in [xml|json|jsv|etc]}
        public override object OnPost(Movie movie)
        {
            var newMovie = new MovieResponse { Movie = movie };

            _moviesList.Add(movie);

            return new HttpResult(newMovie)
            {
                StatusCode = HttpStatusCode.Created,
                Headers = { { HttpHeaders.Location, this.RequestContext.AbsoluteUri.WithTrailingSlash() + movie.Id } }
            };
        }

        /// PUT /movies/{id}
        public override object OnPut(Movie movie)
        {
            // save to db

            if (movie != null) 
            {
                var m = _moviesList.Find(x => x.Id == movie.Id);
                if  (m != null)
                {
                    m.Title = movie.Title;
                    m.Rating = movie.Rating;
                    m.Director = movie.Director;
                    m.ReleaseDate = movie.ReleaseDate;
                    m.Genres = movie.Genres;
                }
            }

            return null;
        }

        /// DELETE /movies/{Id}
        public override object OnDelete(Movie request)
        {
            // delete from db
            if (request != null)
            {
                var m = _moviesList.Find(x => x.Id == request.Id);
                if (m != null)
                {
                    _moviesList.Remove(m);
                }
            }

            return null;
        }
    }

    [Description("Find movies by genre, or all movies if no genre is provided")]
    public class Movies
    {
        public string Genre { get; set; }
    }

    public class MoviesResponse
    {
        //public List<Movie> Movies { get; set; }

        public List<Movie> Movies
        {
            get { return MovieService._moviesList; }
            set { MovieService._moviesList = value; }
        }
    }

    public class MoviesService : RestServiceBase<Movies>
    {
        /// GET /movies 
        /// GET /movies/genres/{Genre}
        public override object OnGet(Movies request)
        {
//            return new MoviesResponse { Movies = new List<Movie>() { new Movie() { Id = 10 } } };
            return MovieService._moviesList;
        }
    }
}
