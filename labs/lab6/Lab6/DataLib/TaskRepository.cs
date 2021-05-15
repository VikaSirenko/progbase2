using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace lab6
{
    public class TaskRepository
    {
        private SqliteConnection connection;
        public TaskRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public long Insert(Task task)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO tasks (topic, description,maxScore, isPublished, publishedAt )
                VALUES ($topic, $description,$maxScore, $isPublished, $publishedAt);
                SELECT last_insert_rowid();
                ";
            command.Parameters.AddWithValue("$topic", task.topic);
            command.Parameters.AddWithValue("$description", task.description);
            command.Parameters.AddWithValue("$maxScore", task.maxScore);
            command.Parameters.AddWithValue("$isPublished", task.isPublished);
            command.Parameters.AddWithValue("$publishedAt", task.publishedAt.ToString("o"));
            long newId = (long)command.ExecuteScalar();
            connection.Close();
            return newId;
        }

        public bool Delete(long id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM tasks WHERE id =$id";
            command.Parameters.AddWithValue("$id", id);
            int nChanges = command.ExecuteNonQuery();
            connection.Close();
            return nChanges == 1;
        }

        public bool Update(Task updatedTask, long taskId)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE tasks SET topic=$topic, description=$description , maxScore=$maxScore , isPublished=$isPublished WHERE id=$id";
            command.Parameters.AddWithValue("$id", taskId);
            command.Parameters.AddWithValue("$topic", updatedTask.topic);
            command.Parameters.AddWithValue("$description", updatedTask.description);
            command.Parameters.AddWithValue("$maxScore", updatedTask.maxScore);
            command.Parameters.AddWithValue("$isPublished", updatedTask.isPublished);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            return nChanged == 1;
        }


        private Task ParseTask(SqliteDataReader reader)
        {
            Task task = new Task();
            int trueOrFalse;
            bool isId = long.TryParse(reader.GetString(0), out task.id);
            bool isBoolValue = int.TryParse(reader.GetString(4), out trueOrFalse);
            bool isScore = double.TryParse(reader.GetString(3), out task.maxScore);
            bool isDate = DateTime.TryParse(reader.GetString(5), out task.publishedAt);

            if (isId && isBoolValue && isScore && isDate)
            {
                task.id = long.Parse(reader.GetString(0));
                task.topic = reader.GetString(1);
                task.description = reader.GetString(2);
                task.maxScore = double.Parse(reader.GetString(3));
                if (trueOrFalse == 1)
                {
                    task.isPublished = true;

                }

                else if (trueOrFalse == 0)
                {
                    task.isPublished = false;
                }

                task.publishedAt = DateTime.Parse(reader.GetString(5));
                return task;
            }

            else
            {
                throw new FormatException("Values cannot be parsed");
            }
        }

        public List<Task> GetAll()
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tasks";
            SqliteDataReader reader = command.ExecuteReader();
            List<Task> tasksList = new List<Task>();
            while (reader.Read())
            {
                Task task = ParseTask(reader);
                tasksList.Add(task);
            }
            reader.Close();
            connection.Close();
            return tasksList;
        }

        private long GetCount()
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM tasks";
            long count = (long)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public int GetTotalPages(int pageLength)
        {
            return (int)Math.Ceiling(this.GetCount() / (double)pageLength);
        }

        public List<Task> GetPageTask(int pageNumber, int pageLength)
        {
            connection.Open();

            if (pageNumber < 1)
            {
                throw new ArgumentException(nameof(pageNumber));
            }

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM tasks LIMIT $pageLength OFFSET $pageLength *($pageNumber -1 ) ";
            command.Parameters.AddWithValue("$pageLength", pageLength);
            command.Parameters.AddWithValue("$pageNumber", pageNumber);
            SqliteDataReader reader = command.ExecuteReader();
            List<Task> tasksList = new List<Task>();

            while (reader.Read())
            {
                Task task = ParseTask(reader);
                tasksList.Add(task);
            }

            reader.Close();
            connection.Close();
            return tasksList;

        }




    }
}