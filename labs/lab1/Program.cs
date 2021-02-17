using System;
using static System.Console;
using static System.IO.File;
using System.IO;
using System.Diagnostics;


class Program
{
    struct FileDescription
    {
        public string path;
        public int number_of_lines;
        public string error;

    }
    static void Main(string[] args)
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        FileDescription inputFile = ParseFile(args);

        if (inputFile.error != "")
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("Error:{0}", inputFile.error);
            ResetColor();
            Environment.Exit(1);
        }

        else
        {
            if (File.Exists(inputFile.path))
            {
                string[,] table = GenerateTable(inputFile);
                string csv_text = TableToCsvText(table);
                WriteAllText(inputFile.path, csv_text);
            }

            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Input file does not exist: create input file");
                ResetColor();

            }

        }

        string path1 = "/home/vika/Documents/task2.1.csv";
        string path2 = "/home/vika/Documents/task2.2.csv";
        string path3 = "/home/vika/Documents/result.csv";

        Random random = new Random();

        FileDescription first_file_task2 = new FileDescription
        {
            path = path1,
            number_of_lines = random.Next(10, 21)
        };

        FileDescription second_file_task2 = new FileDescription
        {
            path = path2,
            number_of_lines = random.Next(100000, 200001)
        };

        // generate a file for 10-20 entities

        string[,] table1 = GenerateTable(first_file_task2);
        string csv_text1 = TableToCsvText(table1);
        WriteAllText(first_file_task2.path, csv_text1);

        // generate a file for 100_000-200_000 entities

        string[,] table2 = GenerateTable(second_file_task2);
        string csv_text2 = TableToCsvText(table2);
        WriteAllText(second_file_task2.path, csv_text2);


        ListPost list1 = ReadAllPosts(path1);
        ListPost list2 = ReadAllPosts(path2);
        OutputResults(list1);
        OutputResults(list2);
        ListPost mixList = DeleteIdenticalIdentifiers(list1, list2);
        mixList = RemoveAllLargerVal(mixList);
        WriteAllPosts(path3, mixList);
        ForegroundColor = ConsoleColor.Green;
        WriteLine();
        WriteLine("OK.");
        ResetColor();

        st.Stop();
        WriteLine(st.Elapsed);


    }
    static FileDescription ParseFile(string[] args)
    {
        FileDescription descriptions = new FileDescription
        {
            path = "",
            number_of_lines = 0,
            error = ""
        };




        if (args.Length != 2)
        {
            descriptions.error = $"the number of arguments must be-2 , but you have - {args.Length} ";
            return descriptions;
        }



        else
        {
            bool n = int.TryParse(args[1], out descriptions.number_of_lines);
            if (n && descriptions.number_of_lines > 0)
            {
                descriptions.path = args[0];

            }
            else
            {
                descriptions.error = $"{nameof(descriptions.number_of_lines)} entered incorrectly";
                return descriptions;

            }

        }



        return descriptions;
    }
    static string[,] GenerateTable(FileDescription file)
    {
        int rows = file.number_of_lines;
        int colums = 4;
        string[,] csvTable = new string[rows + 1, colums];
        csvTable[0, 0] = "ID";
        csvTable[0, 1] = "User name";
        csvTable[0, 2] = "The text of the post";
        csvTable[0, 3] = "Likes";
        int id = 0;
        Random random = new Random();

        for (int i = 1; i < csvTable.GetLength(0); i++)
        {
            for (int j = 0; j < csvTable.GetLength(1); j++)
            {
                if (j == 0)
                {
                    id = random.Next(1, rows);
                    csvTable[i, j] = id.ToString();
                }

                else if (j == 1)
                {
                    csvTable[i, j] = FindLineInFile("./name");
                }

                else if (j == 2)
                {

                    csvTable[i, j] = FindLineInFile("./posts");
                }

                else if (j == 3)
                {
                    int likes = random.Next(0, 10000);
                    csvTable[i, j] = likes.ToString();

                }
            }
        }

        return csvTable;
    }
    static string FindLineInFile(string path)
    {
        Random random = new Random();
        StreamReader reader = new StreamReader(path);
        string line = "";

        // count the lines in the file
        int count = 0;

        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
            {
                break;
            }

            else
            {
                count++;
            }

        }

        reader.Close();

        //randomly select the file line
        int number_user_name = random.Next(1, count + 1);

        //find the generated line in the given file
        StreamReader finder = new StreamReader(path);
        string search_line = "";
        while (number_user_name > 0)
        {
            search_line = finder.ReadLine();
            number_user_name--;
        }

        finder.Close();
        return search_line;


    }

    static string TableToCsvText(string[,] table)
    {
        string[] rows = new string[table.GetLength(0)];
        string[] colums = new string[table.GetLength(1)];
        for (int i = 0; i < table.GetLength(0); i++)
        {
            for (int j = 0; j < table.GetLength(1); j++)
            {
                colums[j] = table[i, j];

            }

            rows[i] = string.Join(",", colums);
        }

        string csv_text = string.Join("\n", rows);
        return csv_text;
    }


    static ListPost ReadAllPosts(string filePath)
    {

        ListPost newlist = new ListPost();
        int count = 0;
        StreamReader reader = new StreamReader(filePath);
        string line = "";
        while (true)
        {
            count++;
            if (count > 1)
            {
                line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                string[] arrayOfOnePost = line.Split(',');
                if (arrayOfOnePost.Length > 4 && arrayOfOnePost.Length <= 0)
                {
                    throw new ArgumentException("The CSV text contains mistakes");
                }

                else
                {

                    Post post = new Post();
                    bool id = int.TryParse(arrayOfOnePost[0], out post.id);
                    bool likes = int.TryParse(arrayOfOnePost[3], out post.likes);
                    if (id && likes)
                    {
                        post.id = int.Parse(arrayOfOnePost[0]);
                        post.username = arrayOfOnePost[1];
                        post.status = arrayOfOnePost[2];
                        post.likes = int.Parse(arrayOfOnePost[3]);
                    }

                    else
                    {
                        throw new ArgumentException("The CSV text contains mistakes");

                    }
                    newlist.AddPost(post);

                }
            }
            else
            {
                line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

            }


        }

        reader.Close();
        return newlist;
    }

    static void OutputResults(ListPost list)
    {
        ForegroundColor = ConsoleColor.Green;
        WriteLine($"List size: {list.Size}");
        ResetColor();
        Post[] posts = list.Posts;
        for (int i = 0; i < 10; i++)
        {
            WriteLine(posts[i]);
            WriteLine();

        }

        WriteLine();

    }


    static ListPost DeleteIdenticalIdentifiers(ListPost list1, ListPost list2)
    {
        ListPost mixList = new ListPost();
        bool[] usedId = new bool[Math.Max(list1.Size, list2.Size)];
        int count = 0;
        for (int i = 0; i < list1.Size; i++)
        {
            if (usedId[list1[i].id])
            {
                continue;

            }
            else
            {

                mixList.AddPost(list1.Posts[i]);
                usedId[list1[i].id] = true;
                count++;
            }
        }
        for (int j = 0; j < list2.Size; j++)
        {

            if (usedId[list2[j].id])
            {
                continue;

            }
            else
            {

                mixList.AddPost(list2.Posts[j]);
                usedId[list2[j].id] = true;
                count++;

            }

        }

        mixList.Size = count;
        return mixList;



    }

    static int FindArithmeticalMean(ListPost list)
    {

        int sum = 0;
        for (int i = 0; i < list.Size; i++)
        {
            sum += list.Posts[i].likes;
        }

        int result = sum / list.Size;
        WriteLine("Arithmetic value of likes:");
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(result);
        ResetColor();
        return result;

    }


    static ListPost RemoveAllLargerVal(ListPost list)
    {
        int arithmetMean = FindArithmeticalMean(list);

        for (int i = 0; i < list.Size; i++)
        {
            if (list.Posts[i].likes < arithmetMean)
            {
                list.DeleteAt(i);
                i--;
            }

        }

        return list;

    }

    static void WriteAllPosts(string filePath, ListPost posts)
    {
        StreamWriter writer = new StreamWriter(filePath);
        for (int i = 0; i < posts.Size; i++)
        {
            string line = $"{posts.Posts[i].id}, {posts.Posts[i].username}, {posts.Posts[i].status}, {posts.Posts[i].likes} ";
            writer.WriteLine(line);
            line = "";
        }

        writer.Close();

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
    public int Size
    {
        get
        {
            return _size;
        }

        set
        {
            if (value < 0)
            {
                throw new ArgumentException($"{Size} negative");
            }
            this._size = value;

        }

    }
    public Post[] Posts
    {
        get
        {
            return _posts;
        }

        set
        {
            this._posts = value;
        }
    }

    public Post this[int index]
    {
        get
        {
            return _posts[index];
        }
        set
        {
            _posts[index] = value;
        }

    }

    public ListPost()
    {
        this._posts = new Post[20];
        this._size = 0;
    }

    public void AddPost(Post newPost)
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

    public void DeleteAt(int index)
    {
        if (index < 0 || index > this._size)
        {
            throw new ArgumentException($"{nameof(index)} is incorrect");
        }

        else
        {
            for (int i = index; i < this.Size; i++)
            {
                _posts[i] = _posts[i + 1];
            }
            _size = _size - 1;

        }
    }

}


