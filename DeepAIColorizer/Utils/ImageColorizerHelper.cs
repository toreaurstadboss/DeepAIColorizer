using System.Text.Json;

namespace DeepAIColorizer.Utils
{

    /// <summary>
    /// Helper utility class for colorizing black and white images using the DeepAI API.
    /// </summary>
    /// <remarks>
    /// Note: DeepAI's colorization API does not expose fine-grained control over the
    /// colorization process (e.g., hue adjustment, saturation levels, or color palette selection).
    /// The API uses a trained model that automatically applies colorization based on learned patterns.
    /// </remarks>
    public class ImageColorizerHelper
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageColorizerHelper"/> class.
        /// </summary>
        /// <param name="apiKey">The DeepAI API key for authentication</param>
        /// <exception cref="ArgumentException">Thrown when apiKey is null or whitespace</exception>
        public ImageColorizerHelper(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
            }

            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
        }

        /// <summary>
        /// Colorizes a black and white image using the DeepAI colorization API.
        /// </summary>
        /// <param name="inputPath">Path to the input black and white image</param>
        /// <param name="outputPath">Path where the colorized image will be saved</param>
        /// <exception cref="FileNotFoundException">Thrown when input file does not exist</exception>
        /// <exception cref="HttpRequestException">Thrown when API request fails</exception>
        /// <exception cref="InvalidOperationException">Thrown when API response is invalid</exception>
        public async Task ColorizeImageAsync(string inputPath, string outputPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"Input image not found: {inputPath}");
            }

            // Prepare multipart form data with the image
            using var form = new MultipartFormDataContent();
            var imageBytes = await File.ReadAllBytesAsync(inputPath);
            form.Add(new ByteArrayContent(imageBytes), "image", Path.GetFileName(inputPath));

            Console.WriteLine("⏳ Sending image to DeepAI for colorization...");

            // Send request to DeepAI API
            var response = await _httpClient.PostAsync("https://api.deepai.org/api/colorizer", form);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"📡 Received response from DeepAI");

            // Parse JSON response to extract the output URL
            var result = JsonDocument.Parse(jsonResponse);
            if (!result.RootElement.TryGetProperty("output_url", out var urlElement))
            {
                throw new InvalidOperationException("DeepAI response missing 'output_url' property.");
            }

            var outputUrl = urlElement.GetString();
            if (string.IsNullOrWhiteSpace(outputUrl))
            {
                throw new InvalidOperationException("DeepAI returned an empty output URL. The image may have been rejected.");
            }

            Console.WriteLine($"🌐 Output URL: {outputUrl}");
            Console.WriteLine("⏳ Downloading colorized image...");

            // Download the colorized image
            var colorizedBytes = await _httpClient.GetByteArrayAsync(outputUrl);
            
            // Ensure output directory exists
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the colorized image
            await File.WriteAllBytesAsync(outputPath, colorizedBytes);
            Console.WriteLine($"💾 Saved colorized image ({colorizedBytes.Length:N0} bytes)");
        }

        /// <summary>
        /// Disposes of the underlying HttpClient.
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }

    }
}