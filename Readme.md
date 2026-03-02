# DeepAI Image Colorizer

A console application that colorizes black and white images using the DeepAI Colorization API.

## Features

- 🎨 Automatic colorization of grayscale images
- 🔐 Secure API key management via `.env` file
- 🚀 Simple command-line interface
- 📁 Automatic output file naming with timestamps
- ✨ Progress indicators and colored console output

## Prerequisites

- .NET 10 or later
- DeepAI API key (get one at [https://deepai.org](https://deepai.org))

## Installation

1. Clone or download this repository
2. Restore NuGet packages:

# DeepAI Image Colorization CLI 

The executable will be in `bin/Release/net10.0/`

## Troubleshooting

### "API key not found" error
- Ensure `.env` file exists in the project root
- Verify the file contains `DEEPAI_API_KEY=your_key`
- Alternatively, pass the key via `--apikey` flag

### "Input file not found" error
- Check the file path is correct
- Use absolute paths if relative paths don't work
- Ensure the file has a valid extension (.jpg, .jpeg, .png)

### HTTP 401 Unauthorized
- Your API key may be invalid or expired
- Get a new key from https://deepai.org

### HTTP 429 Too Many Requests
- You've exceeded the API rate limit
- Wait a few minutes before trying again
- Consider upgrading your DeepAI plan

## License

Standard MIT License. See [LICENSE](License.txt) for details.

## Credits

Powered by [DeepAI](https://deepai.org) Colorization API.

### Command-Line Arguments

| Argument | Alias | Required | Description |
|----------|-------|----------|-------------|
| `--input` | `-i` | Yes | Path to the black & white image (PNG/JPG) |
| `--output` | `-o` | No | Path for the colorized output image |
| `--apikey` | - | No | DeepAI API key (overrides .env file) |
| `--help` | `-h` | No | Display help message |

## Examples

Run for example with the following flag for an old photograph:

`--input old_ships_trondheim_1960s_norway.png`

Input photo:

![Old Ships Trondheim 1960S Norway](DeepAIColorizer/old_ships_trondheim_1960s_norway.png)

Resulting AI colorized photo:

![Old Ships Trondheim 1960S Norway Colorized 20260302 022057](DeepAIColorizer/old_ships_trondheim_1960s_norway_colorized_20260302_022057.png)


## Obtain Deep AI API Key

Go to [https://deepai.org](https://deepai.org)

[Obtain Deep AI API key by creating an account](https://deepai.org/dashboard/images)

