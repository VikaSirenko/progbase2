using System;
using static System.Console;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data;


class Program
{
    static void Main(string[] args)
    {
        string databaseFileName = "./data";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        connection.Open();
        PostRepository repository = new PostRepository(connection);
        ConnectionState state = connection.State;

        bool do_command = true;
        while (do_command)
        {
            if (state == ConnectionState.Open)
            {
                WriteLine($"\nWrite command:");
                string command = ReadLine();
                string[] subcommand = command.Split(' ');

                switch (subcommand[0])
                {
                    case "getById":
                        ProcessGetById(subcommand, repository);
                        break;
                    case "deleteById":
                        ProcessDeleteByID(subcommand, repository);
                        break;
                    case "insert":
                        ProcessInsert(subcommand, repository);
                        break;
                    case "getTotalPages":
                        ProcessGetTotalPages(subcommand, repository);
                        break;
                    case "getPage":
                        ProcessGetPage(subcommand, repository);
                        break;
                    case "export":
                        ProcessExport(subcommand, repository);
                        break;
                    case "exit":
                        do_command = false;
                        break;
                    default:
                        Exception exception = new PlatformNotSupportedException($"Command [{command}] not found");
                        WriteLine(exception);
                        break;
                }
            }

            else
            {
                WriteLine("Error connecting to data base");
                do_command = false;

            }
        }

        connection.Close();

    }




    static void ProcessGetById(string[] subcommand, PostRepository repository)
    {
        if (subcommand.Length != 2)
        {
            Exception exception = new ArgumentOutOfRangeException($"There should be 2 parts, but you have [{subcommand.Length}]");
            WriteLine(exception);
        }

        else
        {

            int id;
            bool isId = int.TryParse(subcommand[1], out id);
            if (id >= 0 && isId == true)
            {
                Post post = repository.GetById(id);
                if (post == null)
                {
                    WriteLine($"Post with id:[{id}] NOT found");
                }

                else
                {
                    WriteLine(post);
                }
            }

            else
            {
                Exception ex = new FormatException($"The value of [{nameof(id)}]  cannot be parsed");
                WriteLine(ex);
            }

        }

    }



    static void ProcessDeleteByID(string[] subcommand, PostRepository repository)
    {

        if (subcommand.Length != 2)
        {
            Exception exception = new ArgumentOutOfRangeException($"There should be 2 parts, but you have [{subcommand.Length}]");
            WriteLine(exception);
        }

        else
        {
            int id;
            bool isId = int.TryParse(subcommand[1], out id);
            if (id >= 0 && isId == true)
            {
                int nChanges = repository.DeleteById(id);
                if (nChanges == 0)
                {
                    WriteLine($"Post with id: [{id}] NOT deleted");
                }

                else
                {
                    WriteLine($"Post with id:[{id}] deleted\nQuantity of deleted posts:[{nChanges}]");
                }
                
            }

            else
            {
                Exception ex = new FormatException($"The value of [{nameof(id)}]  cannot be parsed");
                WriteLine(ex);
            }

        }

    }



    static void ProcessInsert(string[] subcommand, PostRepository repository)
    {
        if (subcommand.Length != 2)
        {
            Exception exception = new ArgumentOutOfRangeException($"There should be 2 parts, but you have [{subcommand.Length}]");
            WriteLine(exception);
        }

        else
        {
            string[] partOfPostData = subcommand[1].Split(",");

            if (partOfPostData.Length != 3)
            {
                WriteLine("Data for new post entered incorrectly");

            }

            else
            {
                int likes;
                bool isLikes = int.TryParse(partOfPostData[2], out likes);
                if (isLikes == false || likes < 0)
                {
                    Exception ex = new FormatException($"The value of {nameof(likes)}  cannot be parsed");
                    WriteLine(ex);

                }

                else
                {
                    Post post = new Post();
                    post.username = partOfPostData[0];
                    post.status = partOfPostData[1];
                    post.likes = likes;
                    long newId = repository.Insert(post);
                    WriteLine($"Id of the new post:[{newId}]");
                }

            }


        }

    }

    static void ProcessGetTotalPages(string[] subcommand, PostRepository repository)
    {
        if (subcommand.Length != 1)
        {
            Exception exception = new ArgumentOutOfRangeException($"There should be 1 part, but you have [{subcommand.Length}]");
            WriteLine(exception);
        }

        else
        {
            long numberOfPages = repository.GetTotalPages();
            WriteLine($"Number of pages in a database with 10 posts: [{numberOfPages}]");

        }

    }



    static void ProcessGetPage(string[] subcommand, PostRepository repository)
    {
        if (subcommand.Length != 2)
        {
            Exception exception = new ArgumentOutOfRangeException($"There should be 2 parts, but you have [{subcommand.Length}]");
            WriteLine(exception);
        }

        else
        {
            int pageNumber;
            bool isPageNumber = int.TryParse(subcommand[1], out pageNumber);
            if (pageNumber > 0 && isPageNumber == true && pageNumber < repository.GetTotalPages())
            {
                ListPost list = repository.GetPage(pageNumber);
                Post[] posts = list.GetPosts();
                WriteLine();

                for (int i = 0; i < 10; i++)
                {
                    WriteLine(posts[i]);
                    WriteLine();

                }

                WriteLine();
            }

            else
            {
                Exception ex = new FormatException($"The value of [{nameof(pageNumber)}]  cannot be parsed or this page does not exist");
                WriteLine(ex);
            }

        }

    }


    static void ProcessExport(string[] subcommand, PostRepository repository)
    {
        if (subcommand.Length != 2)
        {
            Exception exception = new ArgumentOutOfRangeException($"There should be 2 parts, but you have [{subcommand.Length}]");
            WriteLine(exception);
        }

        else
        {
            string path = "./export.csv";
            string valueX = subcommand[1];
            ListPost posts = repository.GetExport(valueX);
            long numberOfRows = posts.GetSize();
            WritePostsInFile(path, posts);
            WriteLine($"Saved in: [{path}]");
            WriteLine($"Number of exported rows: {numberOfRows}");

        }

    }


    static void WritePostsInFile(string filePath, ListPost posts)
    {
        StreamWriter writer = new StreamWriter(filePath);

        for (int i = 0; i < posts.GetSize(); i++)
        {
            string line = $"{posts.GetPosts()[i].id}, {posts.GetPosts()[i].username}, {posts.GetPosts()[i].status}, {posts.GetPosts()[i].likes} ";
            writer.WriteLine(line);
            line = "";
        }

        writer.Close();

    }

}



class PostRepository
{
    private SqliteConnection connection;

    public PostRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public Post GetById(int id)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM posts WHERE id =$id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            Post post = GetOnePost(reader);
            reader.Close();
            return post;

        }

        else
        {
            reader.Close();
            return null;

        }
    }


    public int DeleteById(int id)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM posts WHERE id =$id";
        command.Parameters.AddWithValue("$id", id);
        int nChanges = command.ExecuteNonQuery();
        return nChanges;

    }

    public long Insert(Post post)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO posts ( userName, status , likes )
        VALUES ($userName, $status, $likes);
        SELECT last_insert_rowid();
        ";

        command.Parameters.AddWithValue("$userName", post.username);
        command.Parameters.AddWithValue("$status", post.status);
        command.Parameters.AddWithValue("likes", post.likes);
        long newID = (long)command.ExecuteScalar();
        return newID;


    }

    public long GetCount()
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM posts";
        long count = (long)command.ExecuteScalar();
        return count;

    }

    public long GetTotalPages()
    {
        const int pageSize = 10;
        return (long)Math.Ceiling(this.GetCount() / (double)pageSize);
    }

    public ListPost GetPage(int pageNumber)
    {
        const int pageSize = 10;
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM posts LIMIT $pageSize OFFSET $pageSize *($pageNumber -1 ) ";
        command.Parameters.AddWithValue("$pageSize", pageSize);
        command.Parameters.AddWithValue("$pageNumber", pageNumber);
        ListPost posts = ReadPostsFromCommand(command);
        return posts;


    }

    public ListPost GetExport(string valueX)
    {

        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT  * FROM  posts WHERE status LIKE '%' || $valueX || '%'";
        command.Parameters.AddWithValue("$valueX", valueX);
        ListPost posts = ReadPostsFromCommand(command);
        return posts;


    }

    static Post GetOnePost(SqliteDataReader reader)
    {

        Post post = new Post();
        try
        {

            post.id = int.Parse(reader.GetString(0));
            post.username = reader.GetString(1);
            post.status = reader.GetString(2);
            post.likes = int.Parse(reader.GetString(3));


        }
        catch (FormatException ex)
        {
            ex = new FormatException($"The values cannot be parsed");
            WriteLine(ex);

        }
        return post;

    }

    static ListPost ReadPostsFromCommand(SqliteCommand command)
    {
        SqliteDataReader reader = command.ExecuteReader();
        ListPost posts = new ListPost();
        while (reader.Read())
        {
            Post post = GetOnePost(reader);
            posts.Add(post);
        }
        reader.Close();
        return posts;
    }

}

class Post
{
    public int id;
    public string username;
    public string status;
    public int likes;
    public Post()
    {
        this.id = 0;
        this.username = "";
        this.status = "";
        this.likes = 0;
    }

    public Post(int id, string username, string status, int likes)
    {
        this.id = id;
        this.username = username;
        this.status = status;
        this.likes = likes;
    }

    public override string ToString()
    {
        return string.Format("ID:{0} \nUser name:{1} \nStatus:{2} \nLikes:{3}",
        this.id,
        this.username,
        this.status,
        this.likes);
    }

}



class ListPost
{
    private Post[] _posts;
    private int _size;

    public int GetSize()
    {
        return _size;
    }

    public Post[] GetPosts()
    {
        return _posts;
    }


    public ListPost()
    {
        this._posts = new Post[16];
        this._size = 0;
    }

    public void Add(Post newPost)
    {
        if (_size == _posts.Length)
        {
            this.Expand();
        }
        _posts[_size] = newPost;
        _size++;

    }
    private void Expand()
    {
        int oldCapacity = this._posts.Length;
        Post[] oldArray = this._posts;
        this._posts = new Post[oldCapacity * 2];
        Array.Copy(oldArray, this._posts, oldCapacity);

    }


}

