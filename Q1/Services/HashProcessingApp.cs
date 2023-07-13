using Q1.Models;
using Q1.Repositories;
using RabbitMQ.Client;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Q1.Services
{
    public class HashService
    {
        private readonly HashRepository _hashRepository;
        private readonly IModel _rabbitMQChannel;

        public HashService(HashRepository hashRepository, IModel rabbitMQChannel)
        {
            _hashRepository = hashRepository;
            _rabbitMQChannel = rabbitMQChannel;
        }

        public void GenerateAndProcessHashes()
        {
            var random = new Random();
            var sha1 = SHA1.Create();

            for (int i = 0; i < 40000; i++)
            {
                var randomBytes = new byte[10];
                random.NextBytes(randomBytes);
                var hashBytes = sha1.ComputeHash(randomBytes);
                var hashValue = BitConverter.ToString(hashBytes).Replace("-", "");

                var hash = new Hash
                {
                    HashValue = hashValue,
                    DateCreated = DateTime.UtcNow
                };

                _hashRepository.Add(hash);

                // Publish hash to RabbitMQ queue
                var body = Encoding.UTF8.GetBytes(hashValue);
                _rabbitMQChannel.BasicPublish(exchange: "", routingKey: "hashes", basicProperties: null, body: body);
            }
        }
    }
}
