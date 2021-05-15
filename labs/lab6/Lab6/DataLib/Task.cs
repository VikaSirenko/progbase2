using System;

namespace lab6
{
    public class Task
    {
        public long id;
        public string topic;
        public string description;
        public double maxScore;
        public bool isPublished;
        public DateTime publishedAt;

        public override string ToString()
        {
            return $"[{id}] | Topic:'{topic}'";
        }

    }

    
}