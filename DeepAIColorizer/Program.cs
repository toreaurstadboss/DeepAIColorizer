using DeepAIColorizer.Utils;
using DotNetEnv;

namespace DeepAIColorizer
{
    /// <summary>
    /// Entry point for the DeepAI Image Colorizer console application.
    /// Supports colorizing black and white images using the DeepAI API.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main entry point. Parses command-line arguments and initiates image colorization.
        /// </summary>
        /// <param name="args">Command-line arguments: --input, --output, --apikey (optional)</param>
        static async Task Main(string[] args)
        {
            // Load environment variables from .env file
            Env.Load();

            var inputPath = GetArgValue(args, "--input") ?? GetArgValue(args, "-i");
            var outputPath = GetArgValue(args, "--output") ?? GetArgValue(args, "-o");
            var apiKey = GetArgValue(args, "--apikey") ?? Environment.GetEnvironmentVariable("DEEPAI_API_KEY");

            // Display help if no arguments provided
            if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
            {
                DisplayHelp();
                return;
            }

            // Validate required arguments
            if (string.IsNullOrWhiteSpace(inputPath))
            {
                Console.WriteLine("❌ Error: --input parameter is required.");
                DisplayHelp();
                return;
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("❌ Error: API key not found. Set DEEPAI_API_KEY in .env file or use --apikey flag.");
                return;
            }

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"❌ Error: Input file not found: {inputPath}");
                return;
            }

            // Generate output path if not provided
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                outputPath = Path.Combine(
                    Path.GetDirectoryName(inputPath) ?? ".",
                    Path.GetFileNameWithoutExtension(inputPath) + $"_colorized_{timeStamp}.png"
                );
            }

            Console.WriteLine("🎨 DeepAI Image Colorizer CLI Tool v0.9");
            Console.WriteLine($"📁 Input:  {inputPath}");
            Console.WriteLine($"📁 Output: {outputPath}");
            Console.WriteLine();

            try
            {
                var helper = new ImageColorizerHelper(apiKey);
                await helper.ColorizeImageAsync(inputPath, outputPath);
                
                Console.WriteLine($"✅ Success! Colorized image saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Retrieves the value of a command-line argument.
        /// </summary>
        /// <param name="args">The command-line arguments array</param>
        /// <param name="flag">The flag to search for (e.g., "--input")</param>
        /// <returns>The value following the flag, or null if not found</returns>
        private static string? GetArgValue(string[] args, string flag)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(flag, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        /// <summary>
        /// Displays usage instructions for the application.
        /// </summary>
        private static void DisplayHelp()
        {
            Console.WriteLine("🎨 DeepAI Image Colorizer");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  DeepAIColorizer --input <path> [--output <path>] [--apikey <key>]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -i, --input <path>    Path to the black & white image (PNG/JPG)");
            Console.WriteLine("  -o, --output <path>   Path for the colorized output (optional)");
            Console.WriteLine("  --apikey <key>        DeepAI API key (optional if set in .env)");
            Console.WriteLine("  -h, --help            Display this help message");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  DeepAIColorizer --input photo.jpg --output colorized.png");
            Console.WriteLine();
            Console.WriteLine("Note: Set DEEPAI_API_KEY in .env file to avoid passing --apikey each time.");
        }
    }
}
