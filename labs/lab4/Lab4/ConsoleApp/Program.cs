using System;
using System.Drawing;
using System.Diagnostics;
using static System.Console;
using System.IO;

static class Program
{
    static void Main(string[] args)
    {
        try
        {
            ArgumentsProcessor.Run(args);
            Environment.Exit(0);

        }

        catch (Exception ex)
        {
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine(ex.Message);
            Environment.Exit(1);
        }

    }
}

static class ArgumentsProcessor
{
    struct ProgramArguments
    {
        public string module;
        public string inputFile;
        public string outputFile;
        public string operation;
        public string[] otherArguments;
    }


    public static void Run(string[] args)
    {
        ProgramArguments programArguments = ParseArguments(args);
        Bitmap inputBitmap = new Bitmap(programArguments.inputFile);

        switch (programArguments.operation)
        {
            case "crop":
                {
                    ProcessCrop(programArguments, inputBitmap);
                    break;
                }
            case "rotate180":
                {
                    ProcessRotate180(programArguments, inputBitmap);
                    break;
                }
            case "extractBlue":
                {
                    ProcessExtractBlue(programArguments, inputBitmap);
                    break;
                }
            case "invertColors":
                {
                    ProcessInvertColors(programArguments, inputBitmap);
                    break;
                }
            case "changeBrightness":
                {
                    ProcessChangeBrightness(programArguments, inputBitmap);
                    break;
                }
        }
    }


    private static ProgramArguments ParseArguments(string[] args)
    {
        ProgramArguments programArguments = new ProgramArguments();
        ValidateArgumentsLength(args.Length);

        programArguments.module = args[0];
        ValidateModule(programArguments.module);


        programArguments.inputFile = args[1];
        ValidateInputFile(programArguments.inputFile);

        programArguments.outputFile = args[2];
        programArguments.operation = args[3];
        ValidateOperation(programArguments.operation);
        programArguments.otherArguments = new string[args.Length - 4];

        for (int i = 0; i < programArguments.otherArguments.Length; i++)
        {
            programArguments.otherArguments[i] = args[i + 4];
        }

        return programArguments;
    }


    private static void ValidateArgumentsLength(int length)
    {
        if (length < 4)
        {
            throw new ArgumentException($"Not enough command line arguments. Expected more than 3, got [{length}] ");
        }
    }


    private static void ValidateModule(string module)
    {
        string[] supportedModules = new string[] { "pixel", "fast" };

        for (int i = 0; i < supportedModules.Length; i++)
        {
            if (supportedModules[i] == module)
            {
                return;
            }
        }

        throw new ArgumentException($"Not supported module [{module}]");
    }


    private static void ValidateInputFile(string file)
    {
        if (System.IO.File.Exists(file) == false)
        {
            throw new ArgumentException($"Input file does not exist : [{file}]");
        }
    }


    private static void ValidateOperation(string operation)
    {
        string[] supportedOperations = new string[] { "crop", "rotate180", "extractBlue", "invertColors", "changeBrightness" };

        for (int i = 0; i < supportedOperations.Length; i++)
        {
            if (supportedOperations[i] == operation)
            {
                return;
            }
        }

        throw new ArgumentException($"Not supported operation: [{operation}]");
    }

    private static IImageEditor DetermineTheModule(ProgramArguments programArguments)
    {
        if (programArguments.module == "fast")
        {
            IImageEditor editor1 = new FastImageEditor();
            return editor1;
        }

        IImageEditor editor2 = new PixelImageEditor();
        return editor2;

    }

    private static void ProcessCrop(ProgramArguments programArguments, Bitmap inputBitmap)
    {

        if (programArguments.otherArguments.Length != 1)
        {
            throw new ArgumentException($"Crop operation should have [5] arguments");
        }

        string cropArguments = programArguments.otherArguments[0];
        Stopwatch cropWatch = new Stopwatch();
        IImageEditor editor = DetermineTheModule(programArguments);
        cropWatch.Start();
        Rectangle cropRect = ParseRectangle(cropArguments);
        Bitmap outputBitmat = editor.Crop(inputBitmap, cropRect);
        cropWatch.Stop();
        WriteLine($"Operation Crop finished in {cropWatch.ElapsedMilliseconds} ms");
        outputBitmat.Save(programArguments.outputFile);

    }


    private static Rectangle ParseRectangle(string rectFormat)
    {
        string[] valuesFromRectFormat = rectFormat.Split('+', 'x');

        if (valuesFromRectFormat.Length != 4)
        {
            throw new ArgumentException("Not all data for constructing a rectangle are given");
        }

        int x;
        int y;
        int width;
        int height;
        bool isWidth = int.TryParse(valuesFromRectFormat[0], out width);
        bool isHeight = int.TryParse(valuesFromRectFormat[1], out height);
        bool isX = int.TryParse(valuesFromRectFormat[2], out x);
        bool isY = int.TryParse(valuesFromRectFormat[3], out y);

        if (isHeight && isHeight && isX && isY)
        {
            return new Rectangle
            {
                Location = new Point(x, y),
                Width = width,
                Height = height,
            };
        }

        throw new ArgumentException("The data for constructing the rectangle are presented in the incorrect format");
    }


    private static void ProcessRotate180(ProgramArguments programArguments, Bitmap inputBitmap)
    {
        if (programArguments.otherArguments.Length != 0)
        {
            throw new ArgumentException($"Rotate180 operation contains unnecessary arguments");
        }

        IImageEditor editor = DetermineTheModule(programArguments);
        Stopwatch rotateWatch = new Stopwatch();
        rotateWatch.Start();
        Bitmap outputBitmat = editor.Rotate180(inputBitmap);
        rotateWatch.Stop();
        WriteLine($"Operation Rotate180 finished in {rotateWatch.ElapsedMilliseconds} ms");
        outputBitmat.Save(programArguments.outputFile);

    }

    private static void ProcessExtractBlue(ProgramArguments programArguments, Bitmap inputBitmap)
    {
        if (programArguments.otherArguments.Length != 0)
        {
            throw new ArgumentException($"ExtractBlue operation contains unnecessary arguments");
        }

        IImageEditor editor = DetermineTheModule(programArguments);
        Stopwatch extractBlueWatch = new Stopwatch();
        extractBlueWatch.Start();
        Bitmap outputBitmap = editor.ExtractBlue(inputBitmap);
        extractBlueWatch.Stop();
        WriteLine($"Operation ExtractBlue finished in {extractBlueWatch.ElapsedMilliseconds} ms");
        outputBitmap.Save(programArguments.outputFile);

    }

    private static void ProcessInvertColors(ProgramArguments programArguments, Bitmap inputBitmap)
    {
        if (programArguments.otherArguments.Length != 0)
        {
            throw new ArgumentException($"InvertColors operation contains unnecessary arguments");
        }

        IImageEditor editor = DetermineTheModule(programArguments);
        Stopwatch invertColorsWatch = new Stopwatch();
        invertColorsWatch.Start();
        Bitmap outputBitmap = editor.InvertColors(inputBitmap);
        invertColorsWatch.Stop();
        WriteLine($"Operation InvertColors finished in {invertColorsWatch.ElapsedMilliseconds} ms");
        outputBitmap.Save(programArguments.outputFile);

    }


    private static void ProcessChangeBrightness(ProgramArguments programArguments, Bitmap inputBitmap)
    {
        if (programArguments.otherArguments.Length != 1)
        {
            throw new ArgumentException($"ChangeBrightness operation should have [5] arguments");
        }

        Stopwatch changeBrightnessWatch = new Stopwatch();
        int brightness;
        bool isBright = int.TryParse(programArguments.otherArguments[0], out brightness);

        if (isBright == false || brightness > 100 || brightness < -100)
        {
            throw new ArgumentException($"The brightness value is set incorrectly");
        }

        IImageEditor editor = DetermineTheModule(programArguments);
        changeBrightnessWatch.Start();
        Bitmap outputBitmap = editor.ChangeBrightness(inputBitmap, brightness);
        changeBrightnessWatch.Stop();
        WriteLine($"Operation ChangeBrightness finished in {changeBrightnessWatch.ElapsedMilliseconds} ms");
        outputBitmap.Save(programArguments.outputFile);

    }

}
