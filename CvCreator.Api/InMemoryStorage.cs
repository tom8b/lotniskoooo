using System.Collections.Generic;
using CvCreator.Api.Model;

namespace CvCreator.Api
{
    public interface IInMemoryStorage
    {
        void AddArticle(Article article);
        void AddConnection(Connection connection);
        void AddAirport(Airport airport);
        List<Article> GetArticles { get; }
        List<Connection> GetConnections { get; }
        List<Airport> GetAirports { get; }

    }

    public class InMemoryStorage : IInMemoryStorage
    {
        private readonly List<Article> _articles = new List<Article>();
        private readonly List<Connection> _connections = new List<Connection>();

        private readonly List<Airport> _airports = new List<Airport>() // airport bedzie zawieral city, nie ma case trzymania city osobno na potrzeby studenckie
        {
            new Airport
            {
                Address = "Test Address", Airport_id = 123, City = new City
                {
                    City_id = 1, Name = "Cracow"
                }
            },
            new Airport
            {
                Address = "Test Address2", Airport_id = 1234, City = new City
                {
                    City_id = 2, Name = "Warsaw"
                }
            }
        };

        public void AddArticle(Article article)
        {
            _articles.Add(article);
        }

        public void AddConnection(Connection connection)
        {
            _connections.Add(connection);
        }

        public void AddAirport(Airport airport)
        {
            _airports.Add(airport);
        }

        public List<Article> GetArticles => _articles;
        public List<Connection> GetConnections => _connections;
        public List<Airport> GetAirports => _airports;
    }
}
